using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.TipoProducto
{
    public class GetTipoProductoDTO
    {
        public int Id { get; set; }
        public bool Estado { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
