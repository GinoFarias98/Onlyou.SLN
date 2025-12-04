using Onlyou.Shared.DTOS.Proveedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pedidos.PedidoItem
{
    public class PedidoItemconProveedorDTO
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }

        // Información COMPLETA del proveedor
        public GetProveedorDTO Proveedor { get; set; }
    }
}
