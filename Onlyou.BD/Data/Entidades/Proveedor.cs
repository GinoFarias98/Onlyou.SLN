using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(CUIT), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Proveedor : EntidadBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "El nombre no debe superar los 128 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La Direccion es obligatoria.")]
        [MaxLength(256, ErrorMessage = "La dirección no debe superar los 256 caracteres.")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "La Razon Social es obligatoria.")]
        [MaxLength(128, ErrorMessage = "La razón social no debe superar los 128 caracteres.")]
        public string RazonSocial { get; set; } = null!;

        [Required(ErrorMessage = "El CUIT es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El CUIT no debe superar los 20 caracteres.")]
        public string CUIT { get; set; } = null!;

        [Required(ErrorMessage = "El Telefono es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El teléfono no debe superar los 20 caracteres.")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El Mail es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [MaxLength(128, ErrorMessage = "El email no debe superar los 128 caracteres.")]
        public string Email { get; set; } = null!;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
