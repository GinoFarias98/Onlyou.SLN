using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Movimiento
{
    public class PostMovimientoDesdePedidoDTO
    {
        public DateTime? FechaDelMovimiento { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PedidoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TipoMovimientoId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Monto { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        public EstadoMovimientoDto EstadoMovimiento { get; set; }

    }

}
