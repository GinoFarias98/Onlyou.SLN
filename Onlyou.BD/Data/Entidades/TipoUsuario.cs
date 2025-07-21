using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class TipoUsuario : EntidadBase
    {
        public string? Nombre { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; } = new List<Usuario>();

    }
}
