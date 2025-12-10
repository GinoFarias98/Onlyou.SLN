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
    [Index(nameof(FechaRealizado))]
    [Index(nameof(Situacion))]
    [Index(nameof(TipoPagoId))]
    [Index(nameof(MovimientoId))]
    [Index(nameof(FechaRealizado), nameof(MovimientoId))]
    public class Pago : EntidadBase
    {

        // Fecha del pago -------------------------------------------------------------

        [Required(ErrorMessage = "La fecha del pago es obligatoria.")]
        [Display(Name = "Fecha realizado",
                 Description = "Día en el que se efectuó el pago.")]
        public DateTime FechaRealizado { get; set; } = DateTime.Now;

        // Monto ----------------------------------------------------------------------

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        [Display(Name = "Monto",
                 Description = "Cantidad de dinero abonada en el pago.")]
        public decimal Monto { get; set; }

        // Situación ------------------------------------------------------------------

        [Required(ErrorMessage = "La situación es obligatoria.")]
        [Display(Name = "Situación",
                 Description = "Estado actual del pago (Ej: Completo, Parcial, Anulado).")]
        public Situacion Situacion { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }


        // FK: Movimiento -------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Movimiento.")]
        [ForeignKey(nameof(Movimiento))]
        public int MovimientoId { get; set; }
        public Movimiento Movimiento { get; set; } = null!;



        [Required(ErrorMessage = "El TipoPago es obligatoria.")]
        [ForeignKey(nameof(TipoPago))]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un TipoPago válido.")]
        [Display(Name = "TipoPago",
         Description = "TipoPago del producto.")]
        public int TipoPagoId { get; set; }
        public TipoPago TipoPago { get; set; } = null!;



        [ForeignKey(nameof(Caja))]
        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;

        //public ICollection<ObservacionPago> Observaciones { get; set; } = new List<ObservacionPago>();



    }

    public enum Situacion
    {
        Completo,
        Pendiente,
        Parcial, 
        Anulado
    }

}
