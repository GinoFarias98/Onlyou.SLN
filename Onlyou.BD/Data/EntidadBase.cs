using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data
{
    public class EntidadBase : IEntidadBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public required string Codigo { get; set; }

        public bool Estado { get; set; } = true;
    }
}
