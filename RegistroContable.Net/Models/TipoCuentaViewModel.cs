using Microsoft.AspNetCore.Mvc;
using RegistroContable.Net.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace RegistroContable.Net.Models
{
    public class TipoCuentaViewModel : IValidatableObject
    {
        public int Id { get; set; }
        /// <summary>
        /// Propiedad Nombre -> Validaciones de tipo annotation
        /// Se validan a la hora de dar submite y e ingresar en el controlador y validar con ModelState
        /// Annotation Attriute [PrimeraLetraMayuscula] Personalizada, creada en carpeta Validaciones
        /// [Remote] validación previa a ingresar al controlador, el objetivo es validar el dato ingresado por el usuario antes de dar submite
        /// </summary>
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage ="La longitud del campo {0} debe estar {2} y {1}")]
        [Display(Name ="Nombre del Tipo Cuenta")]
        [PrimeraLetraMayuscula]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TipoCuentas")]
        public string? Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }

        /// <summary>
        /// Validate se ejecuta cuando se presiona el botón de submite haciendo la validación
        /// Implementa la interfaz IValidatableObject
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Nombre != null && Nombre.Length > 0)
            {
                //Alguna otra validación especifica
                //...
                //Mensaje de error
                yield return new ValidationResult("El campo no puede estar vacío", new[] { nameof(Nombre) });
            }
        }
    }
}
