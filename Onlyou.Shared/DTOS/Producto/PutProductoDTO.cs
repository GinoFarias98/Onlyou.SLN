using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Producto
{
    public class PutProductoDTO
    {
        // Nombre ------------------------------------------------------
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        // Stock -------------------------------------------------------
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo o cero.")]
        public int Stock { get; set; }

        // Descripción -------------------------------------------------
        [StringLength(1000, ErrorMessage = "La descripción no debe superar los 1000 caracteres.")]
        public string? Descripcion { get; set; }

        // Imagen (opcional en PUT) ------------------------------------
        public string? Imagen { get; set; }  // base64 (si se quiere reemplazar)
        public string? ImagenExtension { get; set; } // ".jpg", ".png", ".gif"

        // Costo -------------------------------------------------------
        [Range(0, 1000000, ErrorMessage = "El costo debe ser un número positivo o cero.")]
        public decimal Costo { get; set; }

        // Precio ------------------------------------------------------
        [Range(0, 10000000, ErrorMessage = "El precio debe ser un número positivo o cero.")]
        public decimal Precio { get; set; }

        // FK Proveedor -----------------------------------------------
        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
        public int ProveedorId { get; set; }

        // FK Categoria -----------------------------------------------
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int CategoriaId { get; set; }

        // FK TipoProducto --------------------------------------------
        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de producto válido.")]
        public int TipoProductoId { get; set; }

        // FK Marca ---------------------------------------------------
        [Required(ErrorMessage = "La marca es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca válida.")]
        public int MarcaId { get; set; }


        public List<int> Colores { get; set; } = new();
        public List<int> Talles { get; set; } = new();

    }
}
