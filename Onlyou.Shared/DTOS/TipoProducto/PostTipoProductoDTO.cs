using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.TipoProducto
{
    public class PostTipoProductoDTO
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        public string Nombre { get; set; } = null!;


    }
}
