using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioMovimiento : Repositorio<Movimiento>, IRepositorioMovimiento
    {
        private readonly Context context;

        public RepositorioMovimiento(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Movimiento>> SelectMovimientosPorCajaAsync(int cajaId)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.CajaId == cajaId)
                    .Include(m => m.TipoMovimiento)
                    .Include(m => m.Proveedor)
                    .Include(m => m.Pedido)
                    .Include(m => m.Pagos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Movimiento>> SelectMovimientosPorFechaAsync(DateTime fecha)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.FechaDelMovimiento.Date == fecha.Date)
                    .Include(m => m.TipoMovimiento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Movimiento>> SelectMovimientosPorEstadoAsync(string estado)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.EstadoMovimiento == estado)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<decimal> SelectTotalIngresosPorCajaAsync(int cajaId)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.CajaId == cajaId && m.TipoMovimiento.Nombre == "Ingreso")
                    .SumAsync(m => m.Monto);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<decimal> SelectTotalEgresosPorCajaAsync(int cajaId)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.CajaId == cajaId && m.TipoMovimiento.Nombre == "Egreso")
                    .SumAsync(m => m.Monto);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }



    }
}
