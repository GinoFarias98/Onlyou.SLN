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
    public class PutMovimientoDTO
    {

        public int Id { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public DateTime FechaDelMovimiento { get; set; }

        [Required]
        public int TipoMovimientoId { get; set; }

        public int? ProveedorId { get; set; }

        public int? PedidoId { get; set; }
    }
}
