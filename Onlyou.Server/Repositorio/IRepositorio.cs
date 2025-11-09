using Onlyou.BD.Data;
using System.Linq.Expressions;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorio<E> where E : class, IEntidadBase
    {
        Task<bool> Delete(int id);
        Task<bool> Existe(int id);
        Task<bool> ExistePorCondicion(Expression<Func<E, bool>> predicado);
        Task<List<E>> FiltrarAsync(Dictionary<string, object?> filtros);
        Task<int> Insert(E entidad);
        Task<TDTO> InsertDevuelveDTO<TDTO>(E entidad);
        Task<List<E>> Select();
        Task<List<E>> SelectArchivados();
        Task<E?> SelectById(int id);
        Task<List<E>> SelectPorCondicion(Expression<Func<E, bool>> predicado, bool soloActivos = true);
        Task<bool> UpdateEntidad(int id, E entidad);
        Task<bool> UpdateEstado(int id);
    }
}