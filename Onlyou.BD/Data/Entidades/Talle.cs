using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Talle : EntidadBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "Máximo 128 caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre del talle.")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Productos Talles",
                 Description = "Lista de productos asociados a este talle.")]
        public ICollection<ProductoTalle> ProductosTalles { get; set; } = new List<ProductoTalle>();
    }
}
