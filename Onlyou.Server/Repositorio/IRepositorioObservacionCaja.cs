using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioObservacionCaja : IRepositorio<ObservacionCaja>
    {
        Task<ObservacionCaja> AgregarObservacionAsync(int cajaId, string texto);
        Task<IEnumerable<ObservacionCaja>> ListarObservacionesAsync(int cajaId);
    }
}