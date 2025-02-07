using RegistroContable.Net.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace RegistroContable.Net.Models
{
    public class TipoCuentaViewModel //: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage ="La longitud del campo {0} debe estar {2} y {1}")]
        [Display(Name ="Nombre del Tipo Cuenta")]
        [PrimeraLetraMayuscula]
        public string? Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Nombre != null && Nombre.Length > 0) 
        //    {
        //        var primeraLetra = Nombre[0].ToString();
        //        if(primeraLetra != primeraLetra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayúscula", new[] { nameof(Nombre) });
        //        }
        //    }
        //}
    }
}
