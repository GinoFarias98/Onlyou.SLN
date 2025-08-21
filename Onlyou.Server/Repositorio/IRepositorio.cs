using Onlyou.BD.Data;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorio<E> where E : class, IEntidadBase
    {
        Task<bool> Delete(int id);
        Task<bool> Existe(int id);
        Task<bool> ExisteByCodigo(string codigo);
        Task<int> Insert(E entidad);
        Task<string?> InsertDevuelveCodigo(E entidad);
        Task<TDTO> InsertDevuelveDTO<TDTO>(E entidad);
        Task<List<E>> Select();
        Task<E?> SelectByCod(string codigo);
        Task<E?> SelectById(int id);
        Task<bool> UpdateEntidad(int id, E entidad);
        Task<bool> UpdateEstado(int id, E entidad);
    }
}