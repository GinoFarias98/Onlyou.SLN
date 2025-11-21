using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onlyou.Shared.DTOS.Pedidos.EstadoPedido
{
    public class EstadoPedidoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        // Opcional: Incluir conteo de pedidos si es útil para reporting
        public int CantidadPedidos { get; set; }
    }
}
