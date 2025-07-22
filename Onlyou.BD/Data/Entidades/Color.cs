using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Color : EntidadBase
    {
        public string? Nombre { get; set; }
        public string? Hexadecimal { get; set; }

        public ICollection<ProductoColor>? ProductosColores { get; set; } = new List<ProductoColor>();
    }
}
