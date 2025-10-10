using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Categorias
{
    public class GetCategoriasDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string Imagen { get; set; } = null!;
        public string? ImagenExtension { get; set; }
        public bool Estado { get; set; }
    }
}