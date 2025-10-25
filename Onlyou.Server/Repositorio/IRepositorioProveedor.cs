using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioProveedor : IRepositorio<Proveedor>
    {
        Task EliminarProveedorAsync(int id);
        Task<bool> ExisteProv(string cuit, string email);
        Task<Proveedor?> SelectByCUIT(string cuit);
        Task<Proveedor?> SelectByCuit(string cuit);
        Task<List<Proveedor>> SelectByEmail(string email);
        Task<Proveedor?> SelectByRS(string rs);
        Task<List<Proveedor>> SelectBySimilName(string similName);
    }
}