using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Producto
{
    public class PostProductoDTO
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El Código es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public required string Codigo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número entero positivo o cero.")]
        public int Stock { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no debe superar los 1000 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        public string Imagen { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la extensión de la imagen.")]
        [RegularExpression(@"^\.(jpg|jpeg|png|gif)$", ErrorMessage = "El formato de imagen debe ser .jpg, .jpeg, .png o .gif.")]
        public string ImagenExtension { get; set; } = null!;

        [Range(0, 1000000, ErrorMessage = "El costo debe ser un número positivo o cero.")]
        public decimal Costo { get; set; }

        [Range(0, 10000000, ErrorMessage = "El precio debe ser un número positivo o cero.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de producto válido.")]
        public int TipoProductoId { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca válida.")]
        public int MarcaId { get; set; }



        public List<int> Colores { get; set; } = new();
        public List<int> Talles { get; set; } = new();

    }
}
