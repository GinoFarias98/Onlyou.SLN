using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioMarca : IRepositorio<Marca>
    {
        Task EliminarMarcaAsync(int id);
        Task<Marca?> SelectByName(string name);
        Task<List<Marca>> SelectBySimilName(string similName);
        Task<List<Producto>> SelectProdDesdeMarca(int id);
    }
}