using Onlyou.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Caja
{
    public class PutEstadoCajaDTO
    {
        [Required]
        public EstadoCajaDto estadoCaja { get; set; } // "Abierta", "Cerrada", "Anulada"

        [StringLength(500)]
        public string? Observacion { get; set; }

    }


}
