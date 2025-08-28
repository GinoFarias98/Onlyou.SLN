using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTipoMovimiento
    {
        Task<int> Insert(Movimiento entidad);
        Task<string?> InsertDevuelveCodigo(Movimiento entidad);
        Task<TDTO> InsertDevuelveDTO<TDTO>(Movimiento entidad);
        Task<bool> UpdateEntidad(int id, Movimiento entidad);
        Task<bool> UpdateEstado(int id, Movimiento entidad);
    }
}