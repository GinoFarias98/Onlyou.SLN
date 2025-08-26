using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Proveedor
{
    public class PutProveedorDTO
    {

        [Required(ErrorMessage = "El Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public required string Codigo { get; set; }
        public bool Estado { get; set; } = true;


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "El nombre no debe superar los 128 caracteres.")]
        [Display(Name = "Nombre",
         Description = "Nombre del proveedor.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [MaxLength(256, ErrorMessage = "La dirección no debe superar los 256 caracteres.")]
        [Display(Name = "Dirección",
                 Description = "Dirección del proveedor.")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La razón social es obligatoria.")]
        [MaxLength(128, ErrorMessage = "La razón social no debe superar los 128 caracteres.")]
        [Display(Name = "Razón Social",
                 Description = "Razón social del proveedor.")]
        public string RazonSocial { get; set; } = null!;

        [Required(ErrorMessage = "El CUIT es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El CUIT no debe superar los 20 caracteres.")]
        [Display(Name = "CUIT",
                 Description = "Clave Única de Identificación Tributaria.")]
        public string CUIT { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        [Display(Name = "Teléfono",
                 Description = "Número telefónico del proveedor.")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [MaxLength(128, ErrorMessage = "El email no debe superar los 128 caracteres.")]
        [Display(Name = "Email",
                 Description = "Correo electrónico del proveedor.")]
        public string Email { get; set; } = null!;
    }
}
