using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Usuario : EntidadBase
    {
        public string? UserName { get; set; }
        public string? Contrasenia { get; set; }

        public int TipoUsuarioId { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
    }
}
