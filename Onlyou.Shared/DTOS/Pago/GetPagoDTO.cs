using Onlyou.Shared.DTOS.ObservacionPago;
using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pago
{
    public class GetPagoDTO
    {
        public int Id { get; set; }
        public DateTime FechaRealizado { get; set; }
        public decimal Monto { get; set; }
        public SituacionPagoDto Situacion { get; set; }
        public string? Descripcion { get; set; }
        public bool EsPagoCliente { get; set; }

        // FK
        public int MovimientoId { get; set; }
        public string? MovimientoDescripcion { get; set; } // opcional, info resumida del movimiento

        public int TipoPagoId { get; set; }
        public string? TipoPagoNombre { get; set; } // nombre del tipo de pago
        public List<GetObservacionPagoDTO>? Observaciones { get; set; }
    }
}
