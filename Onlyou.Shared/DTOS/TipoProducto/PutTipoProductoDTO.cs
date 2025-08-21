using Onlyou.Shared.DataValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.TipoProducto
{
    public class PutTipoProductoDTO
    {


        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        public string Nombre { get; set; } = null!;


        // Opcional: incluir estado si permitís activar/desactivar
        public bool Estado { get; set; } = true;

        // Opcional: incluir código si querés que sea editable
        [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Codigo { get; set; }

    }
}
