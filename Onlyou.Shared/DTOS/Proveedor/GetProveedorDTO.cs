using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Proveedor
{
    public class GetProveedorDTO
    {

        public int Id { get; set; }

        public required string Codigo { get; set; }

        public bool Estado { get; set; } = true;

        public string Nombre { get; set; } = null!;

        public string Direccion { get; set; } = null!;


        public string RazonSocial { get; set; } = null!;


        public string CUIT { get; set; } = null!;


        public string Telefono { get; set; } = null!;

 
        public string Email { get; set; } = null!;
    }
}
