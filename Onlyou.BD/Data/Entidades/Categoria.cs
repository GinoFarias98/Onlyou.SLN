using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)] // Índice único para buscar por nombre
    public class Categoria : EntidadBase
    {
        // Nombre -------------------------------------------------------------------

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre único de la categoría.")]
        public string Nombre { get; set; } = null!;

        // Imagen -------------------------------------------------------------------

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        [StringLength(255, ErrorMessage = "La ruta de la imagen no puede superar los 255 caracteres.")]
        [Display(Name = "Imagen",
                 Description = "Ruta o URL de la imagen del producto.")]
        public string Imagen { get; set; } = null!;

        // Productos relacionados ---------------------------------------------------

        [Display(Name = "Productos",
                 Description = "Lista de productos que pertenecen a esta categoría.")]
        public ICollection<Producto>? Productos { get; set; } = new List<Producto>();
    }
}
