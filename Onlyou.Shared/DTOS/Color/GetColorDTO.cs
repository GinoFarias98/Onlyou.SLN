using Onlyou.Shared.DataValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Color
{
    public class GetColorDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public bool Estado { get; set; }
        public string Nombre { get; set; } = null!;
        public string Hexadecimal { get; set; } = null!;

    }
}
