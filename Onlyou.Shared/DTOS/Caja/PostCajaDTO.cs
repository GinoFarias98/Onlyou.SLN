using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Caja
{
    public class PostCajaDTO
    {
        [Required(ErrorMessage = "El saldo inicial es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor que cero.")]
        public decimal SaldoInicial { get; set; }
    }
}
