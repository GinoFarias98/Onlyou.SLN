using Microsoft.EntityFrameworkCore;
using Onlyou.Shared.DataValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)] // Índice único en Nombre
    public class Color : EntidadBase
    {
        // Nombre -------------------------------------------------------------------

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre único del color.")]
        public string Nombre { get; set; } = null!;

        // Hexadecimal ---------------------------------------------------------------

        [Required(ErrorMessage = "El hexadecimal del color es obligatorio.")]
        [StringLength(7, ErrorMessage = "Máximo {1} caracteres.")]
        [EmpiezaConHash] // Valida que empiece con # y sea valor hexadecimal válido
        [Display(Name = "Hexadecimal",
                 Description = "Código hexadecimal del color, debe comenzar con #.")]
        public string Hexadecimal { get; set; } = null!;

        // ProductosColores relacionados --------------------------------------------

        [Display(Name = "Productos colores",
                 Description = "Lista de relaciones entre productos y colores.")]
        public ICollection<ProductoColor>? ProductosColores { get; set; } = new List<ProductoColor>();
    }
}
