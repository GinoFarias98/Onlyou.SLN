using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(ProductoId))]
    [Index(nameof(ProductoId), nameof(ColorId), IsUnique = true)]
    public class ProductoColor
    {

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Producto.")]
        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Color.")]
        public int ColorId { get; set; }

        public Color Color { get; set; } = null!;
    }
}
