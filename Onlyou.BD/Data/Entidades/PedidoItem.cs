using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(PedidoId))]
    [Index(nameof(ProductoId))]
    [Index(nameof(PedidoId), nameof(ProductoId), IsUnique = true)]  // Índice único compuesto para evitar duplicados
    public class PedidoItem : EntidadBase
    {
        // Cantidad -----------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [Display(Name = "Cantidad",
                 Description = "Cantidad del producto en el pedido.")]
        public int Cantidad { get; set; }

        // Precio unitario ----------------------------------------------------------

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        [Display(Name = "Precio unitario",
                 Description = "Precio de venta por unidad.")]
        public decimal PrecioUnitarioVenta { get; set; }

        // Subtotal calculado (no mapeado) -----------------------------------------

        [NotMapped]
        [Display(Name = "Subtotal",
                 Description = "Subtotal calculado (precio unitario x cantidad).")]
        public decimal SubTotal => PrecioUnitarioVenta * Cantidad;

        // FK: Pedido ---------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un pedido.")]
        [ForeignKey(nameof(Pedido))]
        [Display(Name = "Pedido",
                 Description = "Pedido al que pertenece este ítem.")]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        // FK: Producto -------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto.")]
        [ForeignKey(nameof(Producto))]
        [Display(Name = "Producto",
                 Description = "Producto incluido en este ítem.")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        // Nota: Se podría agregar NombreItem si se quisiera almacenar el nombre del producto en el pedido
        // [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        // public string NombreItem { get; set; }
    }
}
