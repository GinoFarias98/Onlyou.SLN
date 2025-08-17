
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioColor : IRepositorio<Color>
    {
        Task<string?> ObtenerHexa(int id);
    }
}