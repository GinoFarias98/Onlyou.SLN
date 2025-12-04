using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pedidos.PedidoItem
{
    public class GetPedidoItemDTO
    {
        public int Id { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitarioVenta { get; set; }
        public decimal SubTotal => PrecioUnitarioVenta * Cantidad;

        public int PedidoId { get; set; }

        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = null!;
        public string? ProductoImagen { get; set; }
        public string? ProductoDescripcion { get; set; }

        // Color elegido para el ítem
        public int? ColorId { get; set; }
        public string? ColorNombre { get; set; }

        // Talle elegido para el ítem
        public int? TalleId { get; set; }
        public string? TalleNombre { get; set; }

        // Información del proveedor del producto
        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }
        public string? ProveedorTelefono { get; set; }
        public string? ProveedorEmail { get; set; }

    }


    public class PostPedidoItemDTO
    {

        public int Cantidad { get; set; }
        public decimal PrecioUnitarioVenta { get; set; }
        public int ProductoId { get; set; }

        public int? ColorId { get; set; }
        public string? ColorNombre { get; set; }

        public int? TalleId { get; set; }
        public string? TalleNombre { get; set; }
    }


    public class PutPedidoItemDTO
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitarioVenta { get; set; }
        public int ProductoId { get; set; }

        public int? ColorId { get; set; }
        public string? ColorNombre { get; set; }

        public int? TalleId { get; set; }
        public string? TalleNombre { get; set; }
    }

    public class PatchPedidoItemDTO
    {
        public int? ColorId { get; set; }
        public int? TalleId { get; set; }
    }


}