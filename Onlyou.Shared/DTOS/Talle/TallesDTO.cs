using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Talle
{
    public class TallesDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public required string Codigo { get; set; }
        public bool Estado { get; set; } = true;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "Máximo 128 caracteres.")]
        [Display(Name = "Nombre",
                Description = "Nombre del talle.")]
        public string Nombre { get; set; } = null!;
    }
}
