using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    public class ProductoColor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        [Required]
        public int ColorId { get; set; }

        public Color? Color { get; set; }
    }
}
