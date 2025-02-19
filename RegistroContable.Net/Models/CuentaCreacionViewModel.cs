using Microsoft.AspNetCore.Mvc.Rendering;

namespace RegistroContable.MVC.Models
{
    public class CuentaCreacionViewModel : CuentaViewModel
    {
        public IEnumerable<SelectListItem> TipoCuentas { get; set; }
    }
}
