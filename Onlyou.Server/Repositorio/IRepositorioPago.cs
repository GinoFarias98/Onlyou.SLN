using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        Task<Pago> CambiarSituacionPagoAsync(int idPago, Situacion nuevaSituacion, string? observacion = null);
        Task<List<Pago>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros);
        Task<List<Pago>> SelectArchivadosConRelaciones();
        Task<List<Pago>> SelectConRelaciones();
        Task<Pago> SelectConRelacionesXId(int id);
        Task<IEnumerable<Pago>> SelectPagosPorMetodoAsync(int TipoPagoId);
        Task<IEnumerable<Pago>> SelectPagosPorMovimientoAsync(int movimientoId);
        Task<IEnumerable<Pago>> SelectPagosPorRangoFechasAsync(DateTime inicio, DateTime fin);
        Task<IEnumerable<Pago>> SelectPagosPorSituacionAsync(Situacion situacion);
        Task<decimal> SelectTotalPagosPorMovimientoAsync(int movimientoId);
    }
}