using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(CarritoId))]               
    [Index(nameof(ProductoId))]               
    [Index(nameof(CarritoId), nameof(ProductoId), IsUnique = true)]  // Índice único compuesto (opcional) para evitar duplicados en el mismo carrito y producto
    public class CarritoItem : EntidadBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        public decimal PrecioUnitario { get; set; }

        [NotMapped]
        public decimal SubTotal => PrecioUnitario * Cantidad;

        // FK y navegación al carrito
        public int CarritoId { get; set; }
        public Carrito? Carrito { get; set; }

        // FK y navegación al producto
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        // Idea para el nombre del item, que deberia ser el nombre del producto x ej
        // [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        // public string NombreItem { get; set; }
    }
}
