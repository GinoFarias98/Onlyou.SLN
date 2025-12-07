using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioPago : Repositorio<Pago>, IRepositorioPago
    {
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly IRepositorioCaja repositorioCaja;
        private readonly IRepositorioMovimiento repositorioMovimiento;

        public RepositorioPago(Context context, IMapper mapper, IRepositorioCaja repositorioCaja, IRepositorioMovimiento repositorioMovimiento) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.repositorioCaja = repositorioCaja;
            this.repositorioMovimiento = repositorioMovimiento;
        }

        public async Task<List<Pago>> SelectConRelaciones()
        {
            return await context.Pagos
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorMovimientoAsync(int movimientoId)
        {
            return await context.Pagos
                .Where(p => p.MovimientoId == movimientoId)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .ToListAsync();
        }

        public async Task<Pago?> SelectConRelacionesXId(int id)
        {
            return await context.Pagos
                .Where(p => p.Id == id)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            return await context.Pagos
                .Where(p => p.FechaRealizado >= inicio && p.FechaRealizado <= fin)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorSituacionAsync(Situacion situacion)
        {
            return await context.Pagos
                .Where(p => p.Situacion == situacion)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorMetodoAsync(int tipoPagoId)
        {
            return await context.Pagos
                .Where(p => p.TipoPagoId == tipoPagoId)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .ToListAsync();
        }

        public async Task<decimal> SelectTotalPagosPorMovimientoAsync(int movimientoId)
        {
            return await context.Pagos
                .Where(p => p.MovimientoId == movimientoId
                    && p.Estado == true
                    && p.Situacion != Situacion.Anulado)
                .SumAsync(p => p.Monto);
        }

        public async Task<List<Pago>> SelectArchivadosConRelaciones()
        {
            return await context.Pagos
                .Where(p => !p.Estado)
                .Include(p => p.TipoPago)
                .Include(p => p.Movimiento)
                .ToListAsync();
        }

        public async Task<List<Pago>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros)
        {
            var pagosFiltrados = await FiltrarAsync(filtros);

            if (!pagosFiltrados.Any())
                return new List<Pago>();

            var ids = pagosFiltrados.Select(p => p.Id).ToList();

            return await context.Pagos
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.TipoPago)
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.Proveedor)
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.Pedido)
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.TipoMovimiento)
                .ToListAsync();
        }





        public async Task<Pago> RegistrarPagoConImpactoAsync(Pago pagoNuevo)
        {
            try
            {
                // 1️⃣ Traer movimiento con relaciones
                var movimiento = await context.Movimientos
                    .Include(m => m.Pagos)
                    .Include(m => m.TipoMovimiento)
                    .FirstOrDefaultAsync(m => m.Id == pagoNuevo.MovimientoId);

                if (movimiento == null)
                    throw new InvalidOperationException("El movimiento no existe.");

                // 2️⃣ Validar caja donde impacta el PAGO
                await repositorioCaja.ValidarCajaHabilitadaParaOperar(pagoNuevo.CajaId);

                // 3️⃣ Validar que no se pague de más
                var totalPagado = movimiento.Pagos
                    .Where(p => p.Estado && p.Situacion != Situacion.Anulado)
                    .Sum(p => p.Monto);

                if (totalPagado + pagoNuevo.Monto > Math.Abs(movimiento.Monto))
                    throw new InvalidOperationException("El pago supera el total del movimiento.");

                // 4️⃣ Crear el pago
                pagoNuevo.FechaRealizado = DateTime.UtcNow;
                pagoNuevo.Situacion = totalPagado + pagoNuevo.Monto == Math.Abs(movimiento.Monto)
                    ? Situacion.Completo
                    : Situacion.Parcial;

                await context.Pagos.AddAsync(pagoNuevo);

                // 5️⃣ Impactar el saldo en la caja correctamente según signo
                var caja = await context.Cajas.FirstAsync(c => c.Id == pagoNuevo.CajaId);

                // ✅ Para egresos, restamos del saldo; para ingresos, sumamos
                caja.SaldoInicial += (movimiento.TipoMovimiento.signo == Signo.Suma ? 1 : -1) * pagoNuevo.Monto;

                // 6️⃣ Recalcular el estado del movimiento usando método central
                await repositorioMovimiento.RecalcularEstadoMovimientoPorPagosAsync(movimiento.Id);

                // 7️⃣ Guardar TODO
                await context.SaveChangesAsync();

                return pagoNuevo;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }







        public async Task<Pago> CambiarSituacionPagoAsync(int idPago, Situacion nuevaSituacion, string? observacion = null)
        {
            try
            {
                var pago = await context.Pagos
                    .Include(p => p.Movimiento)
                    .FirstOrDefaultAsync(p => p.Id == idPago);

                if (pago == null)
                    throw new KeyNotFoundException($"No se encontró un Pago con ID {idPago}");

                // VALIDACIÓN CRÍTICA → usar CajaId del movimiento padre
                await repositorioCaja.ValidarCajaHabilitadaParaOperar(pago.Movimiento.CajaId);

                ValidarTransicionSituacionPago(pago, nuevaSituacion);

                pago.Situacion = nuevaSituacion;

                pago.Estado = nuevaSituacion switch
                {
                    Situacion.Anulado => false,
                    Situacion.Completo => true,
                    Situacion.Parcial => true,
                    _ => pago.Estado
                };


                if (!string.IsNullOrWhiteSpace(observacion))
                {
                    pago.Descripcion += " | | PAGOS ANULADOS AUTOMÁTICAMENTE AL ANULAR EL MOVIMIENTO. | |";
                }

                if (nuevaSituacion == Situacion.Anulado)
                {
                    await context.ObservacionPagos
                        .Where(o => o.PagoId == idPago && o.Estado == true)
                        .ExecuteUpdateAsync(u => u.SetProperty(o => o.Estado, false));
                }

                await context.SaveChangesAsync();
                return pago;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


        private static void ValidarTransicionSituacionPago(Pago pago, Situacion nuevaSituacion)
        {
            if (pago.Situacion == Situacion.Anulado)
                throw new InvalidOperationException("No se puede modificar un pago anulado.");

            if (pago.Situacion == Situacion.Completo && nuevaSituacion != Situacion.Completo)
                throw new InvalidOperationException("No se puede modificar un pago completo.");
        }

    }
}
