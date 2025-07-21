using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class CarritoItem : EntidadBase
    {
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }


        public int CarritoId { get; set; }
        public Carrito? Carrito { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
