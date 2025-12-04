using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(PedidoId))]
    [Index(nameof(ProductoId))]
    [Index(nameof(PedidoId), nameof(ProductoId), IsUnique = false)]
    // Nota: no lo dejo único porque un mismo producto puede comprarse varias veces 
    // con diferentes combinaciones de color/talle.

    public class PedidoItem : EntidadBase
    {

        // CANTIDAD
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [Display(Name = "Cantidad",
                 Description = "Cantidad del producto comprada en este ítem.")]
        public int Cantidad { get; set; }


        // PRECIO UNITARIO (al momento del pedido)
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        [Display(Name = "Precio unitario",
                 Description = "Precio de venta por unidad en el momento del pedido.")]
        public decimal PrecioUnitarioVenta { get; set; }

        // Subtotal (no se guarda en BD)
        [NotMapped]
        [Display(Name = "Subtotal",
                 Description = "Subtotal del ítem (Cantidad x PrecioUnitarioVenta).")]
        public decimal SubTotal => PrecioUnitarioVenta * Cantidad;


        // RELACIÓN CON PEDIDO
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un pedido.")]
        [ForeignKey(nameof(Pedido))]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

   
        // RELACIÓN CON PRODUCTO
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto.")]
        [ForeignKey(nameof(Producto))]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;


        // COLOR (opcional)
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Display(Name = "Color",
                 Description = "Color seleccionado para este ítem del pedido.")]
        public int? ColorId { get; set; }

        public Color? Color { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre del color",
                 Description = "Texto del color tal como lo eligió el cliente (por si cambia luego).")]
        public string? ColorNombre { get; set; }


        // TALLE (opcional)
        // ▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬
        [Display(Name = "Talle",
                 Description = "Talle seleccionado para este ítem del pedido.")]
        public int? TalleId { get; set; }

        public Talle? Talle { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre del talle",
                 Description = "Texto del talle tal como lo eligió el cliente (por si cambia luego).")]
        public string? TalleNombre { get; set; }
    }
}