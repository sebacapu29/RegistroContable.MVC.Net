using Microsoft.AspNetCore.Mvc;
using RegistroContable.Net.Models;

namespace RegistroContable.Net.Controllers
{
    public class TipoCuentasController : Controller
    {
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Crear(TipoCuentaViewModel tipoCuenta)
        {
            return View();
        }
    }
}
