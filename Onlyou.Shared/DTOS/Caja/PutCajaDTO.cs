using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Caja
{
    public class PutCajaDTO
    {

        // Fecha de fin ---------------------------------------------------------------

        [Display(Name = "Fecha de fin",
                 Description = "Fecha en que se cerró o finalizó la caja.")]
        public DateTime? FechaFin { get; set; }

        // Saldo inicial --------------------------------------------------------------

        [Required(ErrorMessage = "El saldo inicial es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor que cero.")]
        [Display(Name = "Saldo inicial",
                 Description = "Monto con el que se inicia la caja.")]
        public decimal SaldoInicial { get; set; }

    }
}
