using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTalle : IRepositorio<Talle>
    {
        Task<List<Talle>> SelectTallePorProducto(int productoId);
    }
}