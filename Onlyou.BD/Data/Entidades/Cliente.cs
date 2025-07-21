using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Cliente : EntidadBase
    {
        public string? Nomnre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }

        public ICollection<Carrito>? Carritos { get; set; } = new List<Carrito>();

    }
}
