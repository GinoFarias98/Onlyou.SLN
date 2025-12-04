using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Services
{
    public interface IMovimientoService
    {
        Task<Movimiento> CambiarEstadoAsync(int id, EstadoMovimiento estado, string? obs);
        Task<Movimiento> CrearMovimientoAsync(Movimiento movimiento);
    }
}