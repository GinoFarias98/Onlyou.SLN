using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioObservacionPedido : IRepositorio<ObservacionPedido>
    {
        Task<ObservacionPedido> AgregarObservacionAsync(int pedidoId, string texto);
        Task<IEnumerable<ObservacionPedido>> ListarObservacionesAsync(int pedidoId);
    }
}