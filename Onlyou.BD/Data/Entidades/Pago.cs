﻿using Microsoft.EntityFrameworkCore;
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
    [Index(nameof(MetodoDePago))]
    [Index(nameof(MovimientoId))]
    [Index(nameof(FechaRealizado), nameof(MovimientoId))]
    public class Pago : EntidadBase
    {

        // Fecha del pago -------------------------------------------------------------

        [Required(ErrorMessage = "La fecha del pago es obligatoria.")]
        [Display(Name = "Fecha realizado",
                 Description = "Día en el que se efectuó el pago.")]
        public DateTime FechaRealizado { get; set; }

        // Monto ----------------------------------------------------------------------

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        [Display(Name = "Monto",
                 Description = "Cantidad de dinero abonada en el pago.")]
        public decimal Monto { get; set; }

        // Situación ------------------------------------------------------------------

        [Required(ErrorMessage = "La situación es obligatoria.")]
        [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Situación",
                 Description = "Estado actual del pago (Ej: Completo, Parcial, Anulado).")]
        public string Situacion { get; set; } = null!;

        // Descripción ----------------------------------------------------------------

        [StringLength(500, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Descripción",
                 Description = "Detalle breve sobre el pago.")]
        public string? Descripcion { get; set; }

        // Método de pago -------------------------------------------------------------

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [StringLength(50, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Método de pago",
                 Description = "Forma en que se realizó el pago (Ej: Efectivo, Transferencia, Tarjeta).")]
        public string MetodoDePago { get; set; } = null!;

        // Es pago de cliente ---------------------------------------------------------

        [Display(Name = "¿Es pago de cliente?",
                 Description = "Indica si este pago fue realizado por un cliente.")]
        public bool EsPagoCliente { get; set; }

        // FK: Movimiento -------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Movimiento.")]
        [ForeignKey(nameof(Movimiento))]
        public int MovimientoId { get; set; }
        public Movimiento Movimiento { get; set; } = null!;

    }
}
