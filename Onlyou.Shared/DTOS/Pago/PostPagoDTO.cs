using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pago
{
    public class PostPagoDTO
    {
        [Required]
        public DateTime FechaRealizado { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [Required]
        public SituacionPagoDto Situacion { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public bool EsPagoCliente { get; set; }

        [Required]
        public int MovimientoId { get; set; }

        [Required]
        public int TipoPagoId { get; set; }
    }
}
