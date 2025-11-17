using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(Nombre), IsUnique = true)]
    public class TipoMovimiento : EntidadBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(128, ErrorMessage = "Máximo 128 caracteres.")]
        [Display(Name = "Nombre",
                 Description = "Nombre del tipo de movimiento.")]
        public string Nombre { get; set; } = null!;

        [MaxLength(256, ErrorMessage = "Máximo 256 caracteres.")]
        [Display(Name = "Nombre",
             Description = "Descripcion del tipo de movimiento.")]
        public string? Descripcion { get; set; }

        [Display(Name = "¿Corresponde a un ingreso?",
                 Description = "Indica si el registro corresponde a un ingreso de dinero o recurso.")]
        public bool EsIngreso { get; set; }

        [Display(Name = "Movimientos",
                 Description = "Lista de movimientos asociados a este tipo.")]
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
    }
}
