using Onlyou.Shared.DTOS.Pago;
using Onlyou.Shared.Enums;
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
        public EstadoMovimientoDto EstadoMovimiento { get; set; }


        // FKs
        public int CajaId { get; set; }

        public int TipoMovimientoId { get; set; }
        public string TipoMovimientoNombre { get; set; } = null!;
        public SignoTipoMovimientoDto TipoMovimientoSigno { get; set; }

        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }
        public string? RazonSocial { get; set; }

        public int? PedidoId { get; set; }
        public string? PedidoDescripcion { get; set; }

        public decimal TotalPagado { get; set; }
        public decimal SaldoPendiente { get; set; }

        // Pagos
        public List<GetPagoDTO> Pagos { get; set; } = new();
    }
}
