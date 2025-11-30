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

        public async Task<Movimiento> SelectMovimientoPorIdAsync(int idMovimiento)
        {
            try
            {
                if (idMovimiento <= 0)
                {
                    throw new ArgumentException($"El ID del movimiento debe ser mayor que cero. ID recibido: {idMovimiento}");
                }

                var movimiento = await SelectByIdAsync(idMovimiento);

                if (movimiento == null)
                {
                    throw new KeyNotFoundException($"No se encontró un Movimiento con el ID {idMovimiento}.");
                }

                return movimiento;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }
        public async Task<Movimiento?> SelectByIdAsync(int id)
        {
            return await context.Movimientos
                .Include(m => m.Caja)
                .Include(m => m.TipoMovimiento)
                .Include(m => m.Proveedor)
                .Include(m => m.Pedido)
                .Include(m => m.Pagos)
                .FirstOrDefaultAsync(m => m.Id == id);
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

        public async Task<List<Movimiento>> SelectArchivadosConRelaciones()
        {
            return await context.Movimientos
                .Where(m => !m.Estado)
                .Include(m => m.TipoMovimiento)
                .Include(m => m.Proveedor)
                .Include(m => m.Pedido)
                .Include(m => m.Pagos)
                .ToListAsync();
        }


        public async Task<List<Movimiento>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros)
        {
            try
            {
                var movimientosFiltrados = await FiltrarAsync(filtros);
                var ids = movimientosFiltrados.Select(p => p.Id).ToList();

                var productosConRelaciones = await context.Movimientos
                    .Where(m => ids.Contains(m.Id)) // Ojo: sin AsQueryable()
                    .Include(m => m.TipoMovimiento)
                    .Include(m => m.Proveedor)
                    .Include(m => m.Pedido)
                    .Include(m => m.Pagos)
                    .ToListAsync();

                return productosConRelaciones;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }




        public async Task<Movimiento> CambiarEstadoMovimientoAsync(int IdMovimiento, EstadoMovimiento nuevoEstado, string? Observacion = null)
        {
            try
            {
                var movimiento = await SelectByIdAsync(IdMovimiento);
                if (movimiento == null)
                {
                    throw new KeyNotFoundException($"No se encontro Movimiento con ID {IdMovimiento}");
                }
                ValidarTransicionEstado(movimiento, nuevoEstado);

                movimiento.EstadoMovimiento = nuevoEstado;

                //activo inactivo segun estado
                movimiento.Estado = nuevoEstado switch
                {
                    EstadoMovimiento.Pagado => false,
                    EstadoMovimiento.Anulado => false,
                    _ => true
                };

                // registrar obs
                if (!string.IsNullOrWhiteSpace(Observacion))
                {
                    movimiento.Descripcion += $" | Nota: {Observacion.Trim()}";
                }

                await context.SaveChangesAsync();
                return movimiento;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Metodos Privados

        private static void ValidarTransicionEstado(Movimiento movimiento, EstadoMovimiento nuevoEstado)
        {
            // No se puede modificar un movimiento pagado
            if (movimiento.EstadoMovimiento == EstadoMovimiento.Pagado)
                throw new InvalidOperationException("No se puede modificar un movimiento pagado.");

            // No se puede modificar un movimiento anulado
            if (movimiento.EstadoMovimiento == EstadoMovimiento.Anulado)
                throw new InvalidOperationException("No se puede modificar un movimiento anulado.");

            // Solo se puede pasar de rechazado a pendiente/anulado
            if (movimiento.EstadoMovimiento == EstadoMovimiento.Rechazado &&
                nuevoEstado == EstadoMovimiento.Pagado)
                throw new InvalidOperationException("No se puede pagar un movimiento rechazado.");

        }
    }
}
