using Microsoft.AspNetCore.Mvc;
using RegistroContable.Entities;
using RegistroContable.Infraestructura.Interfaces;
using RegistroContable.MVC.Helpers;
using RegistroContable.MVC.Models;

namespace RegistroContable.MVC.Controllers
{
    public class TipoCuentasController : Controller
    {
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        private readonly IServicioUsuarios _servicioUsuarios;

        public TipoCuentasController(IRepositorioTipoCuentas repositorioTipoCuentas, IServicioUsuarios repositorioUsuarios)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
            _servicioUsuarios = repositorioUsuarios;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId =  _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            var tipoCuentasVM = MapperHelper.MappTipoCuentaDTOToVM(tipoCuentas);
            return View(tipoCuentasVM);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuentaViewModel tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }
            var existeCuenta = await _repositorioTipoCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (existeCuenta)
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre), $"El nombre {tipoCuenta.Nombre} ya existe.");
                return View(tipoCuenta);
            }
            tipoCuenta.UsuarioId =  _servicioUsuarios.ObtenerUsuarioId();
            TipoCuentas tipoCuentaDTO = MapperHelper.MappTipoCuentaVMToDTO(tipoCuenta);
            await _repositorioTipoCuentas.Crear(tipoCuentaDTO);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            int usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await _repositorioTipoCuentas.Existe(nombre, usuarioId);
            if (existeTipoCuenta)
            {
                return Json($"El nombre {nombre} ya existe.");
            }
            return Json(true);
        }

        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEcontrado", "Home");
            }
            await _repositorioTipoCuentas.Borrar(id);
            return View(tipoCuenta);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(TipoCuentas tipoCuentas)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(tipoCuentas.Id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEcontrado", "Home");
            }
            await _repositorioTipoCuentas.Borrar(tipoCuenta.Id);
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuentas tipoCuentas)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(tipoCuentas.Id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEcontrado", "Home");
            }
            await _repositorioTipoCuentas.Actualizar(tipoCuenta);
            return View(tipoCuenta);
        }
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(id, usuarioId);

            if (tipoCuenta == null)
            {
                return RedirectToAction("NoEcontrado", "Home");
            }
            return View(tipoCuenta);
        }
        [HttpPost]
        public async Task<IActionResult> Orden([FromBody] int[] ids)
        {
            var usuarioId = _servicioUsuarios.ObtenerUsuarioId();
            var tiposCuentas = await _repositorioTipoCuentas.Obtener(usuarioId);
            var idsTipoCuentas = tiposCuentas.Select(t => t.Id);
            var idsTiposCuentasNoPertenecenAlUsuario = ids.Except(idsTipoCuentas).ToList();

            if (idsTiposCuentasNoPertenecenAlUsuario.Count > 0)
                return Forbid();

            var tipoCuentasOrdenados = ids.Select((val, i) => new TipoCuentas() { Id = val, Orden = i + 1 }).AsEnumerable();

            await _repositorioTipoCuentas.Ordenar(tipoCuentasOrdenados);

            return Ok();
        }
    }
}
