using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioPedido : IRepositorio<Pedido>
    {
        Task<Pedido?> SelectPedidoPorIdAsync(int pedidoId);
        Task<IEnumerable<Pedido>> SelectPedidosPorClienteAsync(string nombreCliente, int? dni = null);
        //Task<IEnumerable<Pedido>> SelectPedidosPorEstadoEntregaAsync(Pedido.EstadoEntrega estadoEntrega);
        //Task<IEnumerable<Pedido>> SelectPedidosPorEstadoPagoAsync(Pedido.EstadoPago estadoPago);
        Task<IEnumerable<Pedido>> SelectPedidosPorRangoFechasAsync(DateTime inicio, DateTime fin);
        Task<decimal> SelectSaldoPendienteTotalAsync();
    }
}