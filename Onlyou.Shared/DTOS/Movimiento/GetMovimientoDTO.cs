using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Movimiento
{
    public class GetMovimientoDTO
    {
        public int Id { get; set; }

        public DateTime FechaDelMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; } = null!;
        public string EstadoMovimiento { get; set; } = null!;

        // FKs
        public int CajaId { get; set; }
        public string CajaNombre { get; set; } = null!;

        public int TipoMovimientoId { get; set; }
        public string TipoMovimientoNombre { get; set; } = null!;
        public int TipoMovimientoSigno { get; set; }

        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }

        public int? PedidoId { get; set; }
        public string? PedidoDescripcion { get; set; }

        // Pagos
        //public List<GetPagoDTO> Pagos { get; set; } = new();
    }
}
