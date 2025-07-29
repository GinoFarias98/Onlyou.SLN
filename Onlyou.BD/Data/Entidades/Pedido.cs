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
    [Index(nameof(FechaGenerado))]
    [Index(nameof(CarritoId), IsUnique = true)] // Relacion 1 - 1 reforzada
    [Index(nameof(EstadoPedidoId))] 
    public class Pedido : EntidadBase
    {
        [Required(ErrorMessage = "La fecha de generación es obligatoria.")]
        public DateTime FechaGenerado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor que cero.")]
        public decimal MontoTotal { get; set; }

        [MaxLength(256, ErrorMessage = "La descripción no debe superar los 256 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de pedido al proveedor es obligatoria.")]
        public DateTime FechaPedidoAProveedor { get; set; }

        // FK's

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Carrito.")]
        [ForeignKey(nameof(Carrito))]
        public int CarritoId { get; set; }
        public Carrito? Carrito { get; set; }



        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un EstadoPedido.")]
        [ForeignKey(nameof(EstadoPedido))]
        public int EstadoPedidoId { get; set; }
        public EstadoPedido? EstadoPedido { get; set; }
    }
}
