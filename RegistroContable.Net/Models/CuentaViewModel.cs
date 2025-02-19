using RegistroContable.MVC.Validaciones;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RegistroContable.MVC.Models
{
    public class CuentaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        [Display(Name = "Nombre del Tipo Cuenta")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        [Display(Name = "Tipo Cuenta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Descripcion { get; set; }
    }
}
