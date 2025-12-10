using Onlyou.Shared.DTOS.Movimiento;
using Onlyou.Shared.DTOS.ObservacionCaja;
using Onlyou.Shared.Enums;
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
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaFin { get; set; } = DateTime.Now;
        public decimal SaldoInicial { get; set; }
        public EstadoCajaDto EstadoCaja { get; set; }
        public bool Estado { get; set; }
        public decimal SaldoActual { get; set; }

        // ============================
        // movimientos y obs asociados
        // ============================

        public List<GetMovimientoDTO>? Movimientos { get; set; }
        public List<GetObservacionCajaDTO>? Observaciones { get; set; }


    }
}
