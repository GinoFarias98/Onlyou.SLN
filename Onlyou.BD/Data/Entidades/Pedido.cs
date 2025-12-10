using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onlyou.BD.Data.Entidades
{
    [Index(nameof(FechaGenerado))]
    [Index(nameof(EstadoPedidoId))]
    public class Pedido : EntidadBase
    {
        // === Datos generales ===

        [Required(ErrorMessage = "La fecha de generación es obligatoria.")]
        [Display(Name = "Fecha de generación",
                 Description = "Fecha en la que se generó el pedido.")]
        public DateTime FechaGenerado { get; set; } = DateTime.Now;



        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor que cero.")]
        [Display(Name = "Monto total",
                 Description = "Monto total del pedido.")]
        public decimal MontoTotal { get; set; }



        [MaxLength(256, ErrorMessage = "La descripción no debe superar los 256 caracteres.")]
        [Display(Name = "Descripción",
                 Description = "Descripción opcional del pedido.")]
        public string? Descripcion { get; set; }



        [Required(ErrorMessage = "La fecha de pedido al proveedor es obligatoria.")]
        [Display(Name = "Fecha de pedido al proveedor",
                 Description = "Fecha en la que se realizó el pedido al proveedor.")]
        public DateTime FechaPedidoAProveedor { get; set; } = DateTime.Now;



        // === Datos del cliente ===



        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Nombre del cliente",
                 Description = "Nombre del cliente que realizó el pedido.")]
        public string NombreCliente { get; set; } = null!;



        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La dirección debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Dirección del cliente",
                 Description = "Dirección del cliente.")]
        public string DireccionCliente { get; set; } = null!;



        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La localidad debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Localidad",
                 Description = "Localidad del cliente.")]
        public string Localidad { get; set; } = null!;



        [Range(1000000, 99999999, ErrorMessage = "Ingrese un DNI válido.")]
        [Display(Name = "DNI",
                 Description = "Documento Nacional de Identidad del cliente.")]
        public int DNI { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La localidad debe tener entre 3 y 100 caracteres.")]
        [Display(Name = "Telefono",
         Description = "Telefono del cliente.")]
        public string Telefono { get; set; } = string.Empty;


        // === Estado de Entrega ===



        [Required]
        [Display(Name = "Estado de entrega",
                 Description = "Estado de entrega del pedido.")]
        public EstadoEntrega EstadoEntrega { get; set; } = EstadoEntrega.NoEntregado;



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto entregado",
                 Description = "Valor de la mercadería entregada.")]
        public decimal MontoEntregado { get; set; }



        // === Estado de Pago ===



        [Required]
        [Display(Name = "Estado de pago",
                 Description = "Estado de pago del pedido.")]
        public EstadoPago EstadoPago { get; set; } = EstadoPago.NoPagado;



        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto pagado",
                 Description = "Monto pagado hasta el momento.")]
        public decimal MontoPagado { get; set; }



        [NotMapped]
        [Display(Name = "Saldo pendiente",
                 Description = "Saldo pendiente de pago calculado.")]
        public decimal SaldoPendiente => MontoTotal - MontoPagado;



        // === Relación con EstadoPedido ===



        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un EstadoPedido.")]
        [ForeignKey(nameof(EstadoPedido))]
        [Display(Name = "Estado del pedido",
                 Description = "Estado actual del pedido.")]
        public int EstadoPedidoId { get; set; }



        public EstadoPedido EstadoPedido { get; set; } = null!;



        // === Relación con ítems del pedido ===



        [Display(Name = "Items del pedido",
                 Description = "Lista de ítems que conforman el pedido.")]
        public ICollection<PedidoItem> PedidoItems { get; set; } = new List<PedidoItem>();



        // === Relación opcional con movimientos de caja ===



        [Display(Name = "Movimientos de caja",
                 Description = "Movimientos de caja asociados a este pedido.")]
        public ICollection<Movimiento> Movimientos { get; set; } = new List<Movimiento>();


        public ICollection<ObservacionPedido> Observaciones { get; set; } = new List<ObservacionPedido>();



    }



    // === Enumeraciones ===



    public enum EstadoEntrega
    {
        NoEntregado = 0,
        ListoParaEntregar = 1,
        Completo = 2
    }

    public enum EstadoPago
    {
        NoPagado = 0,
        Parcial = 1,
        Pagado = 2
    }
}
