using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioPago : Repositorio<Pago>, IRepositorioPago
    {
        private readonly Context context;

        public RepositorioPago(Context context) : base(context)
        {
            this.context = context;
        }

        // Obtener pagos por movimiento
        public async Task<IEnumerable<Pago>> SelectPagosPorMovimientoAsync(int movimientoId)
        {
            try
            {
                return await context.Pagos
                    .Where(p => p.MovimientoId == movimientoId)
                    .Include(p => p.Movimiento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener pagos por rango de fechas
        public async Task<IEnumerable<Pago>> SelectPagosPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                return await context.Pagos
                    .Where(p => p.FechaRealizado >= inicio && p.FechaRealizado <= fin)
                    .Include(p => p.Movimiento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener pagos por situación (Ej: Completo, Parcial, Anulado)
        public async Task<IEnumerable<Pago>> SelectPagosPorSituacionAsync(string situacion)
        {
            try
            {
                return await context.Pagos
                    .Where(p => p.Situacion == situacion)
                    .Include(p => p.Movimiento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener pagos por método de pago (Efectivo, Transferencia, Tarjeta, etc.)
        public async Task<IEnumerable<Pago>> SelectPagosPorMetodoAsync(string metodo)
        {
            try
            {
                return await context.Pagos
                    .Where(p => p.MetodoDePago == metodo)
                    .Include(p => p.Movimiento)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Total de pagos por movimiento
        public async Task<decimal> SelectTotalPagosPorMovimientoAsync(int movimientoId)
        {
            try
            {
                return await context.Pagos
                    .Where(p => p.MovimientoId == movimientoId)
                    .SumAsync(p => p.Monto);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


    }
}
