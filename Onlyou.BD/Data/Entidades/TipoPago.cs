using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre))]
    public class TipoPago : EntidadBase
    {
        // Nombre -------------------------------------------------------------------

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(128, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre único de la marca.")]
        public string Nombre { get; set; } = null!;

        // Productos relacionados ---------------------------------------------------

        [Display(Name = "Pagos",
                 Description = "Lista de Pagos asociados a este Tipo de Pago.")]
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
