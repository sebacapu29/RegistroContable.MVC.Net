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
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuentaViewModel tipoCuenta)
        {
            if (!ModelState.IsValid) 
            {
                return View(tipoCuenta);
            }
            TipoCuentas tipoCuentaDTO = MappTipoCuenta(tipoCuenta);
            this._repositorioTipoCuentas.Crear(tipoCuentaDTO);
            return View();
        }
        private TipoCuentas MappTipoCuenta(TipoCuentaViewModel tipoCuentaViewModel) {
            return new TipoCuentas { Nombre = tipoCuentaViewModel.Nombre, Orden = tipoCuentaViewModel.Orden , UsuarioId=1 };
        }
    }
}
