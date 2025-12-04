using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Talle;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Producto
{
    public class GetProductoDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;
        public int Stock { get; set; }
        public string? Descripcion { get; set; }
        public string Imagen { get; set; } = null!;
        public string? ImagenExtension { get; set; }
        public DateTime FecUltimaModificacion { get; set; }
        public decimal Costo { get; set; }
        public decimal Precio { get; set; }

        public bool PublicadoWeb { get; set; } = false;

        // Relaciones simplificadas ----------------------
        // Datos del proveedor (solo lectura, se cargan automáticamente)
        public int ProveedorId { get; set; }
        public string ProveedorNombre { get; set; } = string.Empty;
        public string? ProveedorTelefono { get; set; }
        public string? ProveedorEmail { get; set; }

        //
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = null!;

        public int TipoProductoId { get; set; }
        public string TipoProductoNombre { get; set; } = null!;

        public int MarcaId { get; set; }
        public string MarcaNombre { get; set; } = null!;

        // IDs para edición
        public List<int> Colores { get; set; } = new();
        public List<int> Talles { get; set; } = new();

        // Datos para mostrar
        public List<string> ColoresNombres { get; set; } = new();
        public List<string> ColoresHex { get; set; } = new();
        public List<string> TallesNombres { get; set; } = new();



        public List<GetColorDTO> ColoresDetalle { get; set; } = new();
        public List<TallesDTO> TallesDetalle { get; set; } = new();


    }

}



