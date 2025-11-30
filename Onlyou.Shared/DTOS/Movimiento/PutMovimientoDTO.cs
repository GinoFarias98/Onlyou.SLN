using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Movimiento
{
    public class PutMovimientoDTO
    {
        [Required]
        public DateTime FechaDelMovimiento { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;

        [Required]
        public EstadoMovimientoDto EstadoMovimiento { get; set; }

        // FKs
        [Required]
        [Range(1, int.MaxValue)]
        public int CajaId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TipoMovimientoId { get; set; }

        [Range(1, int.MaxValue)]
        public int? ProveedorId { get; set; }

        [Range(1, int.MaxValue)]
        public int? PedidoId { get; set; }

        // Pagos (si permitís edición)
       // public List<PutPagoDTO> Pagos { get; set; } = new();
    }
}
