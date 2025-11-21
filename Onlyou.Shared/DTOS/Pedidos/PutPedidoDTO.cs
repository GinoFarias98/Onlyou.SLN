using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Onlyou.Shared.DTOS.Pedidos
{
   public class PutPedidoDTO
    {
        [Required(ErrorMessage = "El monto total es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto total debe ser mayor que cero")]
        public decimal MontoTotal { get; set; }
        
        public string? Descripcion { get; set; }
        
        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string NombreCliente { get; set; } = null!;
        
        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La dirección debe tener entre 3 y 100 caracteres")]
        public string DireccionCliente { get; set; } = null!;
        
        [Required(ErrorMessage = "La localidad es obligatoria")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "La localidad debe tener entre 3 y 100 caracteres")]
        public string Localidad { get; set; } = null!;
        
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Range(1000000, 99999999, ErrorMessage = "Ingrese un DNI válido")]
        public int DNI { get; set; }
        
        [Required(ErrorMessage = "El estado del pedido es obligatorio")]
        public int EstadoPedidoId { get; set; }
        
        // Estados - CAMBIAR A int
        public int EstadoEntrega { get; set; }
        public int EstadoPago { get; set; }
        public decimal MontoPagado { get; set; }
    }
}
