using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Categorias
{
    public class EditarCategoriasDTO
    {
       
            public int Id { get; set; }
            public string? Nombre { get; set; }
            // Imagen 
            public string? Imagen { get; set; }  // base64 (si se quiere reemplazar)
            public string? ImagenExtension { get; set; } // ".jpg", ".png", ".gif"
            //-------------------------------------------------------
            public bool? Estado { get; set; }
        
    }
}
