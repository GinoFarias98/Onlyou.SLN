using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Color
{
    public class PostColorDTO
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El hexadecimal es obligatorio.")]
        [StringLength(7, ErrorMessage = "Máximo {1} caracteres.")]
        public string Hexadecimal { get; set; } = null!;

        [Required(ErrorMessage = "El Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public required string Codigo { get; set; }

    }
}
