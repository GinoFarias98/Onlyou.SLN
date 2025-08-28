using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioPedidoItem : IRepositorio<PedidoItem>
    {
        //Task ActualizarItemAsync(PedidoItem item);
        //Task AgregarItemAsync(PedidoItem item);
        //Task EliminarItemAsync(int pedidoItemId);
        Task<PedidoItem?> SelectItemPorPedidoYProductoAsync(int pedidoId, int productoId);
        Task<IEnumerable<PedidoItem>> SelectItemsPorPedidoAsync(int pedidoId);
        Task<decimal> SelectTotalPedidoAsync(int pedidoId);
    }
}