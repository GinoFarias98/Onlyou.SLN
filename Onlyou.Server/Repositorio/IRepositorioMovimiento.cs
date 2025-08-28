using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioMovimiento : IRepositorio<Movimiento>
    {
        Task<IEnumerable<Movimiento>> SelectMovimientosPorCajaAsync(int cajaId);
        Task<IEnumerable<Movimiento>> SelectMovimientosPorEstadoAsync(string estado);
        Task<IEnumerable<Movimiento>> SelectMovimientosPorFechaAsync(DateTime fecha);
        Task<decimal> SelectTotalEgresosPorCajaAsync(int cajaId);
        Task<decimal> SelectTotalIngresosPorCajaAsync(int cajaId);
    }
}