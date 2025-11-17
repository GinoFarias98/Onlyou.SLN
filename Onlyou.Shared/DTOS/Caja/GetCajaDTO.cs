using Onlyou.Shared.DTOS.ObservacionCaja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Caja
{
    public class GetCajaDTO
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal SaldoInicial { get; set; }
        public string EstadoCaja { get; set; } = string.Empty;
        public bool Estado { get; set; }

        // movimientos asociados
        // public List<MovimientoDto>? Movimientos { get; set; }

        public List<GetObservacionCajaDTO>? Observaciones { get; set; }


    }
}
