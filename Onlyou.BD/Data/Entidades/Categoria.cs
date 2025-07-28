using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)] // Indice para buscar por nombre de categoria
    public class Categoria : EntidadBase
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(128, ErrorMessage ="Maximo de 128 caracteres")]
        public string Nombre { get; set; } = null!;

        public ICollection<Producto>? Productos { get; set; } = new List<Producto>();
    }
}

