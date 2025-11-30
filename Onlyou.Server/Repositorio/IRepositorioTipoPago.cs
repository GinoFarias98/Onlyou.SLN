using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTipoPago : IRepositorio<TipoPago>
    {
        Task EliminarTipoPagoAsync(int id);
        Task<TipoPago> SelectTipoPagosxId(int idTipoPago);
        Task<List<Pago>> SelectPagoDesdeTipoPago(int id);
    }
}