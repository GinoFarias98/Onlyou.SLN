using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioProveedor
    {
        Task<Proveedor?> SelectByCUIT(string cuit);
        Task<Proveedor?> SelectByRS(string rs);
    }
}