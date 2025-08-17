using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioPago
    {
        Task<IEnumerable<Pago>> SelectPagosPorMetodoAsync(string metodo);
        Task<IEnumerable<Pago>> SelectPagosPorMovimientoAsync(int movimientoId);
        Task<IEnumerable<Pago>> SelectPagosPorRangoFechasAsync(DateTime inicio, DateTime fin);
        Task<IEnumerable<Pago>> SelectPagosPorSituacionAsync(string situacion);
        Task<decimal> SelectTotalPagosPorMovimientoAsync(int movimientoId);
    }
}