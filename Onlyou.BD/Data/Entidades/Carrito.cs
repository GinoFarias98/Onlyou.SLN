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
    [Index(nameof(ClienteId))] // índice para búsquedas por cliente
    public class Carrito : EntidadBase
    {
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }


        [Required]
        public bool Pedido { get; set; }

        [Required(ErrorMessage = "El Cliente es una entidad necesaria")]
        public int ClienteId { get; set; }

        [ForeignKey(nameof(ClienteId))]
        public Cliente Cliente { get; set; } = null!;

        public ICollection<CarritoItem>? CarritoItems { get; set; }
    }
}
