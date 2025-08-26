using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Marca
{
    public class GetMarcaDTO
    {
        public int Id { get; set; }
        public required string Codigo { get; set; }
        public bool Estado { get; set; } = true;
        public string Nombre { get; set; } = null!;
    }
}
