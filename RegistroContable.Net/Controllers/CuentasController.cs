using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RegistroContable.Entities;
using RegistroContable.Infraestructura.Interfaces;
using RegistroContable.MVC.Helpers;
using RegistroContable.MVC.Models;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace RegistroContable.MVC.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IServicioUsuarios _servicioUsuarios;
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        private readonly IRepositorioCuentas _repositorioCuentas;
        private readonly IMapper mapper;

        public CuentasController(IServicioUsuarios repositorioUsuarios, IRepositorioTipoCuentas repositorioTipoCuentas, IRepositorioCuentas repositorioCuentas, IMapper mapper)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
            _servicioUsuarios = repositorioUsuarios;
            _repositorioCuentas = repositorioCuentas;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await _repositorioCuentas.Buscar(usuarioId);

            var modelo = cuentasConTipoCuenta
                .GroupBy(x => x.TipoCuenta)
                .Select(group => new IndiceCuentaViewModel
                {
                    TipoCuenta = group.Key,
                    Cuentas = group.AsEnumerable().Select(x => MapperHelper.MappCuentaDTOToVM(x)) 
                }).ToList();
            return View(modelo);
        }
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            var modeloCuentaCreacion = new CuentaCreacionViewModel();
            modeloCuentaCreacion.TipoCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modeloCuentaCreacion);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuentaVM)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.ObtenerPorId(cuentaVM.TipoCuentaId, usuarioId);
            if(tipoCuentas is null)
                return RedirectToAction("NoEncontrado","Home");

            if (!ModelState.IsValid)
            {
                cuentaVM.TipoCuentas = await ObtenerTiposCuentas(usuarioId);
                //return View(cuentaVM);
            }
            var cuentaDTO = MapperHelper.MappCuentaVMToDTO(cuentaVM);
            await _repositorioCuentas.Crear(cuentaDTO);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId) { 
            var tiposCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            return tiposCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }  
        
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            var modelo = mapper.Map<CuentaCreacionViewModel>(cuenta);
            modelo.TipoCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CuentaCreacionViewModel cuentaEditar)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(cuentaEditar.Id, usuarioId);

            if (cuenta is null)
                return RedirectToAction("NoEncontrado", "Home");
            
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(cuentaEditar.TipoCuentaId, usuarioId);

            if (tipoCuenta is null)
                return RedirectToAction("NoEncontrado", "Home");

            await _repositorioCuentas.Actualizar(new Cuenta { Balance = cuentaEditar.Balance, Descripcion = cuentaEditar.Descripcion, Id = cuentaEditar.Id, Nombre = cuentaEditar.Nombre, TipoCuenta = tipoCuenta.Nombre ?? "-"});
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if(cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(cuenta);
        }
        [HttpPost]
        public async Task<IActionResult> BorrarCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var cuenta = await _repositorioCuentas.ObtenerPorId(id, usuarioId);

            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await _repositorioCuentas.Borrar(id);
            return RedirectToAction("Index");
        }
    }
}
