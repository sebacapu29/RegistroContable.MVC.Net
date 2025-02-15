using Microsoft.AspNetCore.Mvc;
using RegistroContable.Domain;
using RegistroContable.Infraestructura.Interfaces;
using RegistroContable.Net.Models;

namespace RegistroContable.Net.Controllers
{
    public class TipoCuentasController : Controller
    {
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        private readonly IRepositorioUsuarios _repositorioUsuarios;

        public TipoCuentasController(IRepositorioTipoCuentas repositorioTipoCuentas, IRepositorioUsuarios repositorioUsuarios)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
            _repositorioUsuarios = repositorioUsuarios;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = await this._repositorioUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await _repositorioTipoCuentas.ObtenerTodas(usuarioId);
            var tipoCuentasVM = tipoCuentas.Select(tc => new TipoCuentaViewModel { Id = tc.Id, Nombre = tc.Nombre, Orden = tc.Orden, UsuarioId = tc.UsuarioId });
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

            TipoCuentas tipoCuentaDTO = await MappTipoCuenta(tipoCuenta);
            this._repositorioTipoCuentas.Crear(tipoCuentaDTO);
            return RedirectToAction("Index");
        }
        private async Task<TipoCuentas> MappTipoCuenta(TipoCuentaViewModel tipoCuentaViewModel) {
            int usuarioId = await this._repositorioUsuarios.ObtenerUsuarioId();
            return new TipoCuentas { Nombre = tipoCuentaViewModel.Nombre, Orden = tipoCuentaViewModel.Orden, UsuarioId = usuarioId };
        }
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            int usuarioId = await this._repositorioUsuarios.ObtenerUsuarioId();
            var existeTipoCuenta = await _repositorioTipoCuentas.Existe(nombre, usuarioId);
            if (existeTipoCuenta) {
                return Json($"El nombre { nombre } ya existe.");
            }
            return Json(true);
        }
  
        public async Task<IActionResult> BorrarTipoCuenta(int id)
        {
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
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
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
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
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
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
            var usuarioId = await _repositorioUsuarios.ObtenerUsuarioId();
            var tipoCuenta = await _repositorioTipoCuentas.ObtenerPorId(id, usuarioId);

            if(tipoCuenta == null)
            {
                return RedirectToAction("NoEcontrado", "Home");
            }
            return View(tipoCuenta);
        }
    }
}
