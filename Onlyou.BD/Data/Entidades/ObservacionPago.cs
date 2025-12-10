using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class ObservacionPago : EntidadBase
    {
        [Required]
        public int PagoId { get; set; }

        [ForeignKey(nameof(PagoId))]
        public Pago Pago { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Texto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
