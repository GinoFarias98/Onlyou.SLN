using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioEstadoPedido : IRepositorio<EstadoPedido>
    {
        Task<List<EstadoPedido>> SelectBySimilName(string similName);
        Task<List<Pedido>> SelectProductoByEstadoPedido(int id);
    }
}