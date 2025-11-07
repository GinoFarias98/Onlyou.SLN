using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Categorias
{
    public class CrearCategoriasDTO
    {
        public string? Nombre { get; set; }


        [Required(ErrorMessage = "La imagen es obligatoria.")]
        public string Imagen { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la extensión de la imagen.")]
        [RegularExpression(@"^\.(jpg|jpeg|png|gif)$", ErrorMessage = "El formato de imagen debe ser .jpg, .jpeg, .png o .gif.")]
        public string ImagenExtension { get; set; } = null!;
        public bool Estado { get; set; } = true;
    }
}
