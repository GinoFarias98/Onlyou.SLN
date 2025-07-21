using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class Proveedor : EntidadBase
    {
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? RazonSocial { get; set; }
        public string? CUIT { get; set; }
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato introducido no es valido")]
        public string? Email { get; set; }

        public ICollection<Producto>? Productos { get; set; } = new List<Producto>();
    }
}
