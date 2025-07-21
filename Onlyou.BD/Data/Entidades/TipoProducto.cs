using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class TipoProducto : EntidadBase
    {
        public string? Nombre { get; set; }

        public ICollection<Producto>? Productos { get; set; } = new List<Producto>();
    }
}
