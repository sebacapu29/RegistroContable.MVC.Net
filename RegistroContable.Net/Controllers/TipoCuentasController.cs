using Microsoft.AspNetCore.Mvc;
using RegistroContable.Domain;
using RegistroContable.Infraestructura.Interfaces;
using RegistroContable.Net.Models;

namespace RegistroContable.Net.Controllers
{
    public class TipoCuentasController : Controller
    {
        private readonly IRepositorioTipoCuentas _repositorioTipoCuentas;
        public TipoCuentasController(IRepositorioTipoCuentas repositorioTipoCuentas)
        {
            _repositorioTipoCuentas = repositorioTipoCuentas;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = 1;
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

            TipoCuentas tipoCuentaDTO = MappTipoCuenta(tipoCuenta);
            this._repositorioTipoCuentas.Crear(tipoCuentaDTO);
            return RedirectToAction("Index");
        }
        private TipoCuentas MappTipoCuenta(TipoCuentaViewModel tipoCuentaViewModel) {
            return new TipoCuentas { Nombre = tipoCuentaViewModel.Nombre, Orden = tipoCuentaViewModel.Orden , UsuarioId=1 };
        }
        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre)
        {
            const int usuarioId = 1;
            var existeTipoCuenta = await _repositorioTipoCuentas.Existe(nombre, usuarioId);
            if (existeTipoCuenta) {
                return Json($"El nombre { nombre } ya existe.");
            }
            return Json(true);
        }
    }
}
