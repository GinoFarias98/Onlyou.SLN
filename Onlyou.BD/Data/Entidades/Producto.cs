using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre))]
    [Index(nameof(Codigo), IsUnique = true)]
    [Index(nameof(MarcaId), nameof(TipoProductoId))] // Índice compuesto para filtros por Marca y Tipo
    public class Producto : EntidadBase
    {

        [Required(ErrorMessage = "El Codigo del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El Codigo debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Codigo",
         Description = "Codigo del producto.")]
        public string Codigo { get; set; } = null!;

        // Nombre -------------------------------------------------------------------

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre del producto.")]
        public string Nombre { get; set; } = null!;

        // Stock --------------------------------------------------------------------

        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser un número entero positivo o cero.")]
        [Display(Name = "Stock",
                 Description = "Cantidad disponible en stock.")]
        public int Stock { get; set; }

        // Descripción ---------------------------------------------------------------

        [StringLength(1000, ErrorMessage = "La descripción no debe superar los 1000 caracteres.")]
        [Display(Name = "Descripción",
                 Description = "Descripción detallada del producto.")]
        public string? Descripcion { get; set; }

        // Imagen -------------------------------------------------------------------

        [Required(ErrorMessage = "La imagen es obligatoria.")]
        [StringLength(255, ErrorMessage = "La ruta de la imagen no puede superar los 255 caracteres.")]
        [Display(Name = "Imagen",
                 Description = "Ruta o URL de la imagen del producto.")]
        public string Imagen { get; set; } = null!;

        // Fecha última modificación -----------------------------------------------

        [Required(ErrorMessage = "La fecha de última modificación es obligatoria.")]
        [Display(Name = "Fecha última modificación",
                 Description = "Fecha en la que se modificó el producto por última vez.")]
        public DateTime FecUltimaModificacion { get; set; } = DateTime.UtcNow;

        // Costo --------------------------------------------------------------------

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000, ErrorMessage = "El costo debe ser un número positivo o cero.")]
        [Display(Name = "Costo",
                 Description = "Costo del producto.")]
        public decimal Costo { get; set; }

        // Precio -------------------------------------------------------------------

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000000, ErrorMessage = "El precio debe ser un número positivo o cero.")]
        [Display(Name = "Precio",
                 Description = "Precio de venta del producto.")]
        public decimal Precio { get; set; }


        // Publicar en Web Si/No -------------------------------------------------------------------


        [Display(Name = "Publicado en Web",
         Description = "Indica si el producto está disponible para la vista de usuarios finales.")]
        public bool PublicadoWeb { get; set; } = false;

        // FK Proveedor -------------------------------------------------------------

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [ForeignKey(nameof(Proveedor))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido.")]
        [Display(Name = "Proveedor",
                 Description = "Proveedor del producto.")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;

        // FK Categoria -------------------------------------------------------------

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [ForeignKey(nameof(Categoria))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        [Display(Name = "Categoría",
                 Description = "Categoría del producto.")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        // FK TipoProducto ----------------------------------------------------------

        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        [ForeignKey(nameof(TipoProducto))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de producto válido.")]
        [Display(Name = "Tipo de producto",
                 Description = "Tipo o clasificación del producto.")]
        public int TipoProductoId { get; set; }
        public TipoProducto TipoProducto { get; set; } = null!;

        // FK Marca -----------------------------------------------------------------

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [ForeignKey(nameof(Marca))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca válida.")]
        [Display(Name = "Marca",
                 Description = "Marca del producto.")]
        public int MarcaId { get; set; }
        public Marca Marca { get; set; } = null!;

        // Relaciones ---------------------------------------------------------------

        [Display(Name = "Items de pedido",
                 Description = "Lista de ítems donde se incluye este producto.")]
        public ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();

        [Display(Name = "Colores",
                 Description = "Colores asociados al producto.")]
        public ICollection<ProductoColor> ProductosColores { get; set; } = new List<ProductoColor>();

        [Display(Name = "Talles",
                 Description = "Talles disponibles para el producto.")]
        public ICollection<ProductoTalle> ProductosTalles { get; set; } = new List<ProductoTalle>();
    }
}
