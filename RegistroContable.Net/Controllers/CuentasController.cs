using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RegistroContable.Entities;
using RegistroContable.Infraestructura.Interfaces;
using RegistroContable.MVC.Helpers;
using RegistroContable.MVC.Models;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace RegistroContable.MVC.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioUsuarios _repositorioUsuarios;
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        private readonly IRepositorioCuentas _repositorioCuentas;
        public CuentasController(IRepositorioUsuarios repositorioUsuarios, IRepositorioTipoCuentas repositorioTipoCuentas, IRepositorioCuentas repositorioCuentas)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioCuentas = repositorioCuentas;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
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
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            var modeloCuentaCreacion = new CuentaCreacionViewModel();
            modeloCuentaCreacion.TipoCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modeloCuentaCreacion);
        }
        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuentaVM)
        {
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
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
    }
}
