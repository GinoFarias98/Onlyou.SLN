using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Producto;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioProducto : IRepositorio<Producto>
    {
        Task<bool> ActualizarPublicadoWeb(int id, bool publicado);
        Task<bool> ExistePorCodigo(string codigo);
        Task<bool> ExistePorNombre(string nombre);
        Task<List<Producto>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros);
        Task<List<Producto>> ObtenerProductosParaWebAsyncConRelaciones();
        Task<Producto?> SelecByCod(string codigo);
        Task<List<Producto>> SelectArchivadosConRelaciones();
        Task<List<Producto>> SelectConRelaciones();
        Task<Producto?> SelectConRelacionesXId(int id);
        Task<List<Producto>> SelectPaginado(int skip, int take);
        Task<List<Talle>> SelectTallesDisponibles(int productoId);
        Task<ProductoOpcionesDTO?> ObtenerOpcionesProducto(int productoId);



    }
}