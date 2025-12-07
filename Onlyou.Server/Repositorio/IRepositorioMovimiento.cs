using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioMovimiento : IRepositorio<Movimiento>
    {
        Task<Movimiento> CambiarEstadoMovimientoAsync(int IdMovimiento, EstadoMovimiento nuevoEstado, string? Observacion = null);
        Task<List<Movimiento>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros);
        Task<List<Movimiento>> SelectArchivadosConRelaciones();
        Task<Movimiento?> SelectByIdAsync(int id);
        Task<IEnumerable<Movimiento>> SelectMovimientosPorCajaAsync(int cajaId);
        //Task<IEnumerable<Movimiento>> SelectMovimientosPorEstadoAsync(string estado);
        Task<IEnumerable<Movimiento>> SelectMovimientosPorFechaAsync(DateTime fecha);
        Task<Movimiento> SelectMovimientoPorIdAsync(int idMovimiento);
        Task<decimal> SelectTotalEgresosPorCajaAsync(int cajaId);
        Task<decimal> SelectTotalIngresosPorCajaAsync(int cajaId);
        Task RecalcularEstadoMovimientoPorPagosAsync(int movimientoId);
        Task RegistrarImpactoEnCajaPorPagoAsync(Movimiento movimiento, decimal monto);
        Task<int> InsertMovimientoConObservacionAsync(Movimiento entidad);
    }
}