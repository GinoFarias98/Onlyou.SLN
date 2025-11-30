using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.ObservacionPago
{
    public class GetObservacionPagoDTO
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}
