using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Carrito : EntidadBase
    {
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public string? Pedido { get; set; }

        public int ClienteID { get; set; }
        public Cliente? Cliente { get; set; }

        public ICollection<CarritoItem>? CarritoItems { get; set; }
    }
}
