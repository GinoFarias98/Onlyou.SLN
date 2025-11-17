using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(FechaInicio))]
    [Index(nameof(FechaFin))]
    [Index(nameof(estadoCaja))]
    public class Caja : EntidadBase
    {
        // Fecha de inicio ------------------------------------------------------------

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [Display(Name = "Fecha de inicio",
                 Description = "Fecha en que se abrió o inició la caja.")]
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

        // Fecha de fin ---------------------------------------------------------------

        [Display(Name = "Fecha de fin",
                 Description = "Fecha en que se cerró o finalizó la caja.")]
        public DateTime? FechaFin { get; set; }

        // Saldo inicial --------------------------------------------------------------

        [Required(ErrorMessage = "El saldo inicial es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El saldo inicial debe ser mayor que cero.")]
        [Display(Name = "Saldo inicial",
                 Description = "Monto con el que se inicia la caja.")]
        public decimal SaldoInicial { get; set; }

        // Estado --------------------------------------------------------------------

        [Required]
        [Display(Name = "Estado de la caja")]
        public EstadoCaja estadoCaja { get; set; } = EstadoCaja.Abierta;
        // Movimientos asociados -----------------------------------------------------

        [Display(Name = "Movimientos",
                 Description = "Lista de movimientos asociados a esta caja.")]
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();

        public ICollection<ObservacionCaja> Observaciones { get; set; } = new List<ObservacionCaja>();


        public enum EstadoCaja
        {
            Abierta,
            Cerrada,
            Anulada
        }

    }
}
