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
    }



    public class PostPedidoItemDTO
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitarioVenta { get; set; }
        public int ProductoId { get; set; }
    }


    public class PutPedidoItemDTO
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitarioVenta { get; set; }
        public int ProductoId { get; set; }
    }


}