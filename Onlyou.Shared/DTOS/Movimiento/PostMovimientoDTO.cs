using Onlyou.Shared.DTOS.Pago;
using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Movimiento
{
    public class PostMovimientoDTO
    {
        [Required]
        public DateTime FechaDelMovimiento { get; set; } = DateTime.Now;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; } = null!;

        [Required]
        public EstadoMovimientoDto EstadoMovimiento { get; set; }

        // FKs obligatorias
        [Required]
        [Range(1, int.MaxValue)]
        public int CajaId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TipoMovimientoId { get; set; }

        // Opcionales
        [Range(1, int.MaxValue)]
        public int? ProveedorId { get; set; }

        [Range(1, int.MaxValue)]
        public int? PedidoId { get; set; }

        // Pagos opcionales
        public List<PostPagoDTO> Pagos { get; set; } = new();
    }
}
