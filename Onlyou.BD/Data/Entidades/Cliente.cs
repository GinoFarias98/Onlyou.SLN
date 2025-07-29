using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre))]
    [Index(nameof(Apellido))]
    [Index(nameof(Nombre), nameof(Apellido))] // Índice compuesto para búsquedas por nombre completo
    public class Cliente : EntidadBase
    {
        [Required(ErrorMessage = "El Nombre es un campo requerido")]
        [StringLength(45, ErrorMessage = "Máximo de caracteres permitido: 45")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El Apellido es un campo requerido")]
        [StringLength(45, ErrorMessage = "Máximo de caracteres permitido: 45")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El Teléfono es un campo requerido")]
        [StringLength(45, ErrorMessage = "Máximo de caracteres permitido: 45")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "El Email es un campo requerido")]
        [EmailAddress(ErrorMessage = "Formato de email no válido, por favor verifique")]
        [StringLength(128, ErrorMessage = "Máximo de caracteres permitido: 128")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La Dirección es un campo requerido")]
        [StringLength(128, ErrorMessage = "Máximo de caracteres permitido: 128")]
        public string Direccion { get; set; } = null!;

        // Relación con carrito
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    }
}
