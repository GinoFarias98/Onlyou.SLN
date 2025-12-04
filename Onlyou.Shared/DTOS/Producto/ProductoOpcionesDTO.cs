using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Producto
{
    public class ProductoOpcionesDTO
    {
        public List<OpcionColorDTO> Colores { get; set; } = new();
        public List<OpcionTalleDTO> Talles { get; set; } = new();
    }
    public class OpcionColorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

    public class OpcionTalleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

}
