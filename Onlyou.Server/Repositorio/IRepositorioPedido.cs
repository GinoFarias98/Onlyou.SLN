using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioPedido : IRepositorio<Pedido>
    {
        Task<Pedido?> SelectPedidoPorIdAsync(int pedidoId);
        Task<IEnumerable<Pedido>> SelectPedidosPorClienteAsync(string nombreCliente, int? dni = null);
        Task<IEnumerable<Pedido>> SelectPedidosPorRangoFechasAsync(DateTime inicio, DateTime fin);
        Task<decimal> SelectSaldoPendienteTotalAsync();

        Task<IEnumerable<Pedido>> GetAllWithDetailsAsync();
        Task<bool> ExistsAsync(int id);

        // Métodos de actualización consistentes
        Task UpdateAsync(Pedido entidad);
    }
}