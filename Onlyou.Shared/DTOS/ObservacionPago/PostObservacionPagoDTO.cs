using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.ObservacionPago
{
    public class PostObservacionPagoDTO
    {
        [Required]
        [StringLength(500)]
        public string Texto { get; set; } = string.Empty;
    }
}
