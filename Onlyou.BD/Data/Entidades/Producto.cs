using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre))] // Indexa el nombre del producto para búsquedas rápidas
    [Index(nameof(MarcaId), nameof(TipoProductoId))] // Index compuesto útil para filtros por marca y tipo
    public class Producto : EntidadBase
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El Nombre debe tener entre 3 y 100 caracteres")]
        public string Nombre { get; set; } = null!;


        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser un número entero positivo o cero.")]
        public int Stock { get; set; }


        [StringLength(1000, ErrorMessage = "La Descripcion debe tener entre 0 y 1000 caracteres")]
        public string? Descripcion { get; set; }


        [StringLength(255)]
        public string Imagen { get; set; } = null!;


        [Required(ErrorMessage = "Fecha obligatoria.")]
        public DateTime FecUltimaModificacion { get; set; } = DateTime.UtcNow;


        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000, ErrorMessage = "El valor debe ser un número positivo o cero.")]
        public decimal Costo { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000000, ErrorMessage = "El valor debe ser un número positivo o cero.")]
        public decimal Precio { get; set; }


        [ForeignKey(nameof(Proveedor))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor.")]
        public int ProveedorId { get; set; }
        public Proveedor Proveedor { get; set; } = null!;


        [ForeignKey(nameof(Categoria))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Categoria.")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;


        [ForeignKey(nameof(TipoProducto))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Producto.")]
        public int TipoProductoId { get; set; }
        public TipoProducto TipoProducto { get; set; } = null!;


        [ForeignKey(nameof(Marca))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Marca.")]
        public int MarcaId { get; set; }
        public Marca Marca { get; set; } = null!;


        // Relaciones

        public ICollection<CarritoItem> CarritoItems { get; set; } = new List<CarritoItem>();
        public ICollection<ProductoColor> ProductosColores { get; set; } = new List<ProductoColor>();
        public ICollection<ProductoTalle> ProductosTalles { get; set; } = new List<ProductoTalle>();

    }
}
