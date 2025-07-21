using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Pedido : EntidadBase
    {
        public DateTime FechaGenerado { get; set; }
        public decimal MontoTotal { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaPedidoAProveedor { get; set; }

        public int CarritoId { get; set; }
        public Carrito? Carrito { get; set; }


        public int EstadoPedidoId { get; set; }
        public EstadoPedido? EstadoPedido { get; set; }
    }
}
