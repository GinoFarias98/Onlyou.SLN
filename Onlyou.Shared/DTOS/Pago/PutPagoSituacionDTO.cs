using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pago
{
    public class PutPagoSituacionDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public SituacionPagoDto NuevaSituacion { get; set; }

        public string? Observacion { get; set; }
    }
}
