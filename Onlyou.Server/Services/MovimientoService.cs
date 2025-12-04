using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;

namespace Onlyou.Server.Services
{
    public class MovimientoService : IMovimientoService
    {
        private readonly IRepositorioMovimiento repoMovimiento;

        public MovimientoService(IRepositorioMovimiento repoMovimiento)
        {
            this.repoMovimiento = repoMovimiento;
        }

        public async Task<Movimiento> CrearMovimientoAsync(Movimiento movimiento)
        {
            movimiento.EstadoMovimiento = EstadoMovimiento.Pendiente;
            movimiento.FechaDelMovimiento = DateTime.UtcNow;

            await repoMovimiento.Insert(movimiento); // devuelve id, pero no lo usamos

            return movimiento; // devolvemos el objeto completo
        }

        public async Task<Movimiento> CambiarEstadoAsync(int id, EstadoMovimiento estado, string? obs)
        {
            return await repoMovimiento.CambiarEstadoMovimientoAsync(id, estado, obs);
        }
    }
}
