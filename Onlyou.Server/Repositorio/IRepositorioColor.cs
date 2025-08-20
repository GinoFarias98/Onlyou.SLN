
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Color;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioColor : IRepositorio<Color>
    {
        Task<GetColorDTO> InsertDevuelveDTO(Color color);
        Task<string?> ObtenerHexa(int id);
    }
}