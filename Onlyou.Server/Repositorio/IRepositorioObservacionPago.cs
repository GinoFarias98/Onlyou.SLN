using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioObservacionPago : IRepositorio<ObservacionPago>
    {
        Task<ObservacionPago> AgregarObservacionAsync(int pagoID, string texto);
        Task<IEnumerable<ObservacionPago>> ListarObservacionesAsync(int pagoID);
    }
}