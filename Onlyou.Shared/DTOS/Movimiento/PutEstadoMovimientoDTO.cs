using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Movimiento
{
    public class PutEstadoMovimientoDTO
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public EstadoMovimientoDto EstadoMovimiento { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = null!;
    }
}
