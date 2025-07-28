using Microsoft.EntityFrameworkCore;
using Onlyou.Shared.DataValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class Color : EntidadBase
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(128, ErrorMessage = "Maximo de 128 caracteres")]
        public string Nombre { get; set; } = null!;


        [Required(ErrorMessage = "El Hexadecimal del Color es obligatorio")]
        [MaxLength(7, ErrorMessage = "Maximo de 7 caracteres")]
        [EmpiezaConHash] //valida que empiece con # y sea valor hexa
        public string Hexadecimal { get; set; } = null!;

        public ICollection<ProductoColor>? ProductosColores { get; set; } = new List<ProductoColor>();
    }
}
