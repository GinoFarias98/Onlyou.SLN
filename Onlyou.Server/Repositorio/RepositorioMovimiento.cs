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
        private readonly IRepositorioCaja repositorioCaja;

        public RepositorioMovimiento(Context context, IMapper mapper, IRepositorioCaja repositorioCaja)
            : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.repositorioCaja = repositorioCaja;
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
                    .Where(m => ids.Contains(m.Id))
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

        // ===================================================
        //  CAMBIAR ESTADO DE MOVIMIENTO  (RECALCULA CAJA)
        // ===================================================
        public async Task<Movimiento> CambiarEstadoMovimientoAsync(
            int IdMovimiento,
            EstadoMovimiento nuevoEstado,
            string? Observacion = null)
        {
            try
            {
                var movimiento = await SelectByIdAsync(IdMovimiento);
                if (movimiento == null)
                    throw new KeyNotFoundException($"No se encontro Movimiento con ID {IdMovimiento}");

                await repositorioCaja.ValidarCajaHabilitadaParaOperar(movimiento.CajaId);

                ValidarTransicionEstado(movimiento, nuevoEstado);

                movimiento.EstadoMovimiento = nuevoEstado;

                movimiento.Estado = nuevoEstado switch
                {
                    EstadoMovimiento.Pagado => false,
                    EstadoMovimiento.Anulado => false,
                    _ => true
                };

                if (nuevoEstado == EstadoMovimiento.Anulado)
                {
                    await context.Pagos
                            .Where(p => p.MovimientoId == movimiento.Id && p.Estado == true)
                            .ExecuteUpdateAsync(u => u
                                .SetProperty(p => p.Estado, false)
                                .SetProperty(p => p.Situacion, Situacion.Anulado)
                            );

                    movimiento.Descripcion +=
                        " | | PAGOS ANULADOS AUTOMÁTICAMENTE AL ANULAR EL MOVIMIENTO. | |";
                }

                if (!string.IsNullOrWhiteSpace(Observacion))
                {
                    movimiento.Descripcion += $" | Nota: {Observacion.Trim()}";
                }

                await context.SaveChangesAsync();

                // 🔥 NUEVO: recalcular caja
                await repositorioCaja.RecalcularSaldoAsync(movimiento.CajaId);

                return movimiento;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // ===================================================
        //  RE-CALCULO DEL ESTADO DEL MOVIMIENTO POR PAGOS
        //  (TAMBIÉN RECALCULA CAJA)
        // ===================================================
        public async Task RecalcularEstadoMovimientoPorPagosAsync(int movimientoId)
        {
            var movimiento = await context.Movimientos
                .Include(m => m.Pagos)
                .FirstOrDefaultAsync(m => m.Id == movimientoId);

            if (movimiento == null)
                throw new InvalidOperationException("Movimiento inexistente.");

            var totalPagado = movimiento.Pagos
                .Where(p => p.Estado && p.Situacion != Situacion.Anulado)
                .Sum(p => p.Monto);

            var totalMovimiento = Math.Abs(movimiento.Monto);

            if (totalPagado == 0)
                movimiento.EstadoMovimiento = EstadoMovimiento.Pendiente;
            else if (totalPagado < totalMovimiento)
                movimiento.EstadoMovimiento = EstadoMovimiento.Parcial;
            else
                movimiento.EstadoMovimiento = EstadoMovimiento.Pagado;

            await context.SaveChangesAsync();

            await repositorioCaja.RecalcularSaldoAsync(movimiento.CajaId);
        }


        // ===================================================
        //  IMPACTO EN CAJA POR PAGO (SUMA / RESTA)
        //  AHORA TAMBIÉN RECALCULA EL SALDO COMPLETO
        // ===================================================
        public async Task RegistrarImpactoEnCajaPorPagoAsync(Movimiento movimiento, decimal monto)
        {
            // Ahora solo recalcula el saldo real
            await repositorioCaja.RecalcularSaldoAsync(movimiento.CajaId);
        }

        public async Task<decimal> SelectTotalRealPorPagosCajaAsync(int cajaId, Signo signo)
        {
            var pagos = await context.Pagos
                .Where(p => p.CajaId == cajaId
                    && p.Estado == true
                    && p.Situacion != Situacion.Anulado)
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.TipoMovimiento)
                .ToListAsync();

            return pagos
                .Where(p => p.Movimiento.TipoMovimiento.signo == signo)
                .Sum(p => p.Monto);
        }



        public async Task<int> InsertMovimientoConObservacionAsync(Movimiento entidad)
        {
            await repositorioCaja.ValidarCajaHabilitadaParaOperar(entidad.CajaId);

            await context.Movimientos.AddAsync(entidad);
            await context.SaveChangesAsync();

            await RecalcularEstadoMovimientoPorPagosAsync(entidad.Id);

            // Obtener nombre del tipo de movimiento seguro
            var tipo = await context.TipoMovimientos
                .Where(t => t.Id == entidad.TipoMovimientoId)
                .Select(t => t.Nombre)
                .FirstAsync();

            var caja = await context.Cajas
                .Include(c => c.Observaciones)
                .FirstAsync(c => c.Id == entidad.CajaId);

            caja.Observaciones.Add(new ObservacionCaja
            {
                Texto = $"Nuevo movimiento registrado: {tipo} por ${Math.Abs(entidad.Monto)}.",
                FechaCreacion = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            return entidad.Id;
        }



        // ===================================================
        //  MÉTODOS PRIVADOS
        // ===================================================

        private static void ValidarTransicionEstado(Movimiento movimiento, EstadoMovimiento nuevoEstado)
        {
            if (movimiento.EstadoMovimiento == EstadoMovimiento.Pagado)
                throw new InvalidOperationException("No se puede modificar un movimiento pagado.");

            if (movimiento.EstadoMovimiento == EstadoMovimiento.Anulado)
                throw new InvalidOperationException("No se puede modificar un movimiento anulado.");

            if (movimiento.EstadoMovimiento == EstadoMovimiento.Rechazado &&
                nuevoEstado == EstadoMovimiento.Pagado)
                throw new InvalidOperationException("No se puede pagar un movimiento rechazado.");
        }
    }
}
