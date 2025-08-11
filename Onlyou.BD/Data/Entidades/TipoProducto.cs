using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class TipoProducto : EntidadBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "Máximo 128 caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre del tipo de producto.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Productos",
                 Description = "Lista de productos asociados a este tipo.")]
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
