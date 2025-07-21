using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Producto : EntidadBase
    {
        public string? Nombre { get; set; }
        public int Stock { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public DateTime FecUltimaModificacion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }


        public int ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int TipoProductoId { get; set; }
        public TipoProducto? TipoProducto { get; set; }

        public int MarcaId { get; set; }
        public Marca? Marca { get; set; }

    }
}
