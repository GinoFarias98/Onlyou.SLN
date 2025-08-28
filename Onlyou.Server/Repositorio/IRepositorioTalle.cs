using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Talle;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTalle : IRepositorio<Talle>
    {
        //Task<IEnumerable<TallesDTO>> Select();
        Task<List<Talle>> SelectTallePorProducto(int productoId);
    }
}