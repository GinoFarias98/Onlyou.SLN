using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioCategoria : IRepositorio<Categoria>
    {
        Task EliminarCategoriaAsync(int id);
        Task<List<Categoria>> SelecBySimilName(string similName);
        Task<List<Producto>> SelectProdByCategoria(int id);
    }
}