using Onlyou.Shared.DTOS.Movimiento;
using Onlyou.Shared.DTOS.ObservacionPedido;
using Onlyou.Shared.DTOS.Pedidos.PedidoItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Onlyou.Shared.DTOS.Pedidos
{
    public class GetPedidosDTO
    {
        public int Id { get; set; }

        [Display(Name = "Fecha de generación")]
        public DateTime FechaGenerado { get; set; }

        [Display(Name = "Monto total")]
        public decimal MontoTotal { get; set; }

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "Fecha pedido a proveedor")]
        public DateTime FechaPedidoAProveedor { get; set; }

        // Datos del cliente
        [Display(Name = "Nombre del cliente")]
        public string NombreCliente { get; set; } = null!;

        [Display(Name = "Dirección")]
        public string DireccionCliente { get; set; } = null!;

        [Display(Name = "Localidad")]
        public string Localidad { get; set; } = null!;

        [Display(Name = "DNI")]
        public int DNI { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;


        // Estados - CAMBIAR A int
        [Display(Name = "Estado de entrega")]
        public int EstadoEntrega { get; set; } // ← 0=NoEntregado, 1=Parcial, 2=Completo

        [Display(Name = "Monto entregado")]
        public decimal MontoEntregado { get; set; }

        [Display(Name = "Estado de pago")]
        public int EstadoPago { get; set; } // ← 0=NoPagado, 1=Parcial, 2=Pagado

        [Display(Name = "Monto pagado")]
        public decimal MontoPagado { get; set; }

        [Display(Name = "Saldo pendiente")]
        public decimal SaldoPendiente => MontoTotal - MontoPagado;

        // Relaciones
        [Display(Name = "Estado del pedido")]
        public int EstadoPedidoId { get; set; }

        [Display(Name = "Estado del pedido")]
        public string EstadoPedidoNombre { get; set; } = null!;


        [Display(Name = "Items del pedido")]
        public List<GetPedidoItemDTO> PedidoItems { get; set; } = new List<GetPedidoItemDTO>();
        //public List<PedidoItemconProveedorDTO> PedidoItemsconProveedor { get; set; } = new List<PedidoItemconProveedorDTO>();


        // observaciones 
        public List<GetObservacionPedidoDTO>? Observaciones { get; set; }

        public List<GetMovimientoDTO>? Movimientos { get; set; }


    }
}
