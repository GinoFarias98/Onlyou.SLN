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
    [Index(nameof(FechaDelMovimiento))]
    [Index(nameof(EstadoMovimiento))]
    [Index(nameof(CajaId))]
    [Index(nameof(TipoMovimientoId))]
    [Index(nameof(ProveedorId))]
    [Index(nameof(PedidoId))]
    public class Movimiento : EntidadBase
    {
        // Fecha del movimiento ------------------------------------------------------

        [Required(ErrorMessage = "La fecha del movimiento es obligatoria.")]
        [Display(Name = "Fecha del movimiento",
                 Description = "Indica el día en que se registró el ingreso o egreso.")]
        public DateTime FechaDelMovimiento { get; set; } = DateTime.Now;

        // Monto ---------------------------------------------------------------------

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        [Display(Name = "Monto",
                 Description = "Cantidad de dinero que ingresa o egresa en el movimiento.")]
        public decimal Monto { get; set; }

        // Descripción ---------------------------------------------------------------

        [StringLength(500, ErrorMessage = "Máximo {1} caracteres.")]
        [Display(Name = "Descripción",
                 Description = "Detalle breve del motivo del movimiento.")]
        public string Descripcion { get; set; } = null!;

        // Estado --------------------------------------------------------------------

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado",
                 Description = "Estado actual del movimiento (Ej: Pendiente, Pagado, Anulado).")]
        public EstadoMovimiento EstadoMovimiento { get; set; }




        // FK: Caja -------------------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una Caja.")]
        [ForeignKey(nameof(Caja))]
        public int CajaId { get; set; }
        public Caja Caja { get; set; } = null!;

        // FK: TipoMovimiento ---------------------------------------------------------

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un Tipo de Movimiento.")]
        [ForeignKey(nameof(TipoMovimiento))]
        public int TipoMovimientoId { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; } = null!;

        // FK: Proveedor --------------------------------------------------------------

        [ForeignKey(nameof(Proveedor))]
        public int? ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        // FK: Pedido -----------------------------------------------------------------

        [ForeignKey(nameof(Pedido))]
        public int? PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        // Pagos ----------------------------------------------------------------------



        [Display(Name = "Pagos",
                 Description = "Lista de pagos asociados a este movimiento.")]
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }


    public enum EstadoMovimiento
    {
        Pendiente,   // El movimiento fue creado pero no se concretó
        Pagado,      // Ya se realizó el pago o ingreso correspondiente
        Anulado,     // Movimiento cancelado
        Parcial,     // Opcional, si se admite pago parcial
        Rechazado    // Opcional, para movimientos que no se validan
    }

}
