using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioProducto : IRepositorio<Producto>
    {
        Task<Producto?> SelecByCod(string codigo);
        Task<List<Producto>> SelectBySimilName(string similName);
        Task<List<Producto>> SelectByTipoProducto(int tipoProductoId);
        Task<List<Producto>> SelectConRelaciones();
        Task<Producto?> SelectConRelacionesXId(int id);
        Task<List<Producto>> SelectPaginado(int skip, int take);
        Task<List<Producto>> SelectProdByMarcaFiltro(int marcaId);
        Task<List<Producto>> SelectProductoByProv(int proveedorId);
        Task<List<Talle>> SelectTallesDisponibles(int productoId);
    }
}