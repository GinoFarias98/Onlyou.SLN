
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Color;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioColor : IRepositorio<Color>
    {
        new Task<TDTO> InsertDevuelveDTO<TDTO>(Color entidad);
        Task<string?> ObtenerHexa(int id);
    }
}