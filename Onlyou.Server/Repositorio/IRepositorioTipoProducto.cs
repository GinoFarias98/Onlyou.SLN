using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTipoProducto : IRepositorio<TipoProducto>
    {
        Task<List<TipoProducto>> SelectBySimilName(string similName);
        Task<List<Producto>> SelectProductosByTipoProd(int id);
    }
}