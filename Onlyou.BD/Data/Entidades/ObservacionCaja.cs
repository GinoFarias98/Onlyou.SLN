using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class ObservacionCaja : EntidadBase
    {
        [Required]
        public int CajaId { get; set; }

        [ForeignKey(nameof(CajaId))]
        public Caja Caja { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Texto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
