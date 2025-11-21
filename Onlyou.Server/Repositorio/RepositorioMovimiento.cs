using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioMovimiento : Repositorio<Movimiento>, IRepositorioMovimiento
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioMovimiento(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
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

        //public async Task<IEnumerable<Movimiento>> SelectMovimientosPorEstadoAsync(string estado)
        //{
        //    try
        //    {
        //        return await context.Movimientos
        //            .Where(m => m.EstadoMovimiento == estado)
        //            .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        ImprimirError(ex);
        //        throw;
        //    }
        //}

        public async Task<decimal> SelectTotalIngresosPorCajaAsync(int cajaId)
        {
            try
            {
                return await context.Movimientos
                    .Where(m => m.CajaId == cajaId && m.TipoMovimiento.signo == Signo.Suma)
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
                    .Where(m => m.CajaId == cajaId && m.TipoMovimiento.signo == Signo.Resta)
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
