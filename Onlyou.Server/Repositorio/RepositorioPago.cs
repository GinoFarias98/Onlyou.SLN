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
            // 1️⃣ Traer movimiento con relaciones
            var movimiento = await context.Movimientos
                .Include(m => m.Pagos)
                .Include(m => m.TipoMovimiento)
                .FirstOrDefaultAsync(m => m.Id == pagoNuevo.MovimientoId);

            if (movimiento == null)
                throw new InvalidOperationException("El movimiento no existe.");

            // 2️⃣ Validar caja habilitada (usa la caja del movimiento)
            var caja = await context.Cajas
                .FirstOrDefaultAsync(c => c.Id == movimiento.CajaId);

            if (caja == null)
                throw new InvalidOperationException("La caja del movimiento no existe.");

            await repositorioCaja.ValidarCajaHabilitadaParaOperar(movimiento.CajaId);

            // 3️⃣ Validar que no se pague de más sobre el movimiento
            var totalPagado = movimiento.Pagos
                .Where(p => p.Estado && p.Situacion != Situacion.Anulado)
                .Sum(p => p.Monto);

            if (totalPagado + pagoNuevo.Monto > Math.Abs(movimiento.Monto))
                throw new InvalidOperationException("El pago supera el total del movimiento.");

            // 4️⃣ VALIDACIÓN CRUCIAL: verificar que haya saldo suficiente en caja para egresos
            if (movimiento.TipoMovimiento.signo == Signo.Resta)
            {
                var saldoActualCaja = await repositorioCaja.RecalcularSaldoAsync(caja.Id); // recalcula saldo real
                if (saldoActualCaja.SaldoActual < pagoNuevo.Monto)
                    throw new InvalidOperationException(
                        $"No hay saldo suficiente en la caja para realizar este egreso. Saldo actual: {saldoActualCaja.SaldoActual}");
            }

            // 5️⃣ Crear el pago
            pagoNuevo.FechaRealizado = DateTime.UtcNow;
            pagoNuevo.CajaId = movimiento.CajaId;

            pagoNuevo.Situacion =
                totalPagado + pagoNuevo.Monto == Math.Abs(movimiento.Monto)
                    ? Situacion.Completo
                    : Situacion.Parcial;

            await context.Pagos.AddAsync(pagoNuevo);

            // 6️⃣ Impactar el saldo de caja según signo del movimiento
            await repositorioMovimiento.RegistrarImpactoEnCajaPorPagoAsync(
                movimiento,
                pagoNuevo.Monto
            );

            // 7️⃣ Recalcular el estado del movimiento por pagos
            await repositorioMovimiento.RecalcularEstadoMovimientoPorPagosAsync(movimiento.Id);

            // 8️⃣ Registrar observación en caja
            var tipo = movimiento.TipoMovimiento?.Nombre ?? "Movimiento";

            caja.Observaciones.Add(new ObservacionCaja
            {
                Texto = $"Pago registrado: ${pagoNuevo.Monto} para {tipo} (Movimiento #{movimiento.Id}).",
                FechaCreacion = DateTime.UtcNow
            });

            // 9️⃣ Guardar todo
            await context.SaveChangesAsync();

            return pagoNuevo;
        }





        public async Task<Pago> CambiarSituacionPagoAsync(int idPago, Situacion nuevaSituacion, string? observacion = null)
        {
            var pago = await context.Pagos
                .Include(p => p.Movimiento)
                .FirstOrDefaultAsync(p => p.Id == idPago);

            if (pago == null)
                throw new KeyNotFoundException($"No se encontró un Pago con ID {idPago}");

            await repositorioCaja.ValidarCajaHabilitadaParaOperar(pago.Movimiento.CajaId);

            ValidarTransicionSituacionPago(pago, nuevaSituacion);

            pago.Situacion = nuevaSituacion;
            pago.Estado = nuevaSituacion != Situacion.Anulado;

            // Observación correcta
            if (!string.IsNullOrWhiteSpace(observacion))
            {
                pago.Descripcion += $" | Nota: {observacion.Trim()}";
            }

            if (nuevaSituacion == Situacion.Anulado)
            {
                await context.ObservacionPagos
                    .Where(o => o.PagoId == idPago && o.Estado == true)
                    .ExecuteUpdateAsync(u => u.SetProperty(o => o.Estado, false));

                // Registrar observación en caja
                var caja = await context.Cajas
                    .Include(c => c.Observaciones)
                    .FirstAsync(c => c.Id == pago.Movimiento.CajaId);

                caja.Observaciones.Add(new ObservacionCaja
                {
                    Texto = $"Pago ANULADO: ${pago.Monto} del Movimiento #{pago.MovimientoId}. Motivo: {observacion}",
                    FechaCreacion = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
                await repositorioCaja.RecalcularSaldoAsync(pago.Movimiento.CajaId);
            }

            await context.SaveChangesAsync();
            return pago;
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
