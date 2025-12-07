using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioCaja : Repositorio<Caja>, IRepositorioCaja
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioCaja(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // ============================
        // MÉTODOS PÚBLICOS
        // ============================



        public async Task<Caja> RecalcularSaldoAsync(int cajaId)
        {
            var caja = await context.Cajas
                .FirstOrDefaultAsync(c => c.Id == cajaId);

            if (caja == null)
                throw new KeyNotFoundException($"No existe la caja con ID {cajaId}");

            // ✅ SOLO PAGOS REALES IMPACTAN EL SALDO
            var pagos = await context.Pagos
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.TipoMovimiento)
                .Where(p => p.CajaId == cajaId
                    && p.Estado == true
                    && p.Situacion != Situacion.Anulado)
                .ToListAsync();

            decimal totalReal = pagos.Sum(p =>
                p.Movimiento.TipoMovimiento.signo == Signo.Suma ? p.Monto :
                p.Movimiento.TipoMovimiento.signo == Signo.Resta ? -p.Monto : 0
            );

            caja.SaldoActual = caja.SaldoInicial + totalReal;

            await context.SaveChangesAsync();

            return caja;
        }




        public async Task ValidarCajaHabilitadaParaOperar(int cajaId)
        {
            var caja = await context.Cajas
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cajaId);

            if (caja == null)
                throw new InvalidOperationException("La caja no existe.");

            if (caja.estadoCaja == Caja.EstadoCaja.Cerrada)
                throw new InvalidOperationException("La caja está cerrada. No se permiten movimientos ni pagos.");

            if (caja.estadoCaja == Caja.EstadoCaja.Anulada)
                throw new InvalidOperationException("La caja está anulada. No se permite ninguna operación.");
        }



        public async Task<Caja?> SelectCajaAbiertaAsync()
        {
            try
            {
                return await context.Cajas
                    .Include(c => c.Movimientos)
                    .Include(c => c.Observaciones)
                    .FirstOrDefaultAsync(c => c.estadoCaja == Caja.EstadoCaja.Abierta);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Caja>> SelectCajasPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                return await context.Cajas
                    .Where(c => c.FechaInicio <= fin && c.FechaFin >= inicio)
                    .Include(c => c.Movimientos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<Caja> AbrirNuevaCajaAsync(decimal saldoInicial)
        {
            await ValidarNoExisteCajaAbiertaAsync();
            var nuevaCaja = new Caja
            {
                FechaInicio = DateTime.UtcNow,
                FechaFin = null,
                estadoCaja = Caja.EstadoCaja.Abierta,
                SaldoInicial = saldoInicial,
                SaldoActual = saldoInicial,   // 🔥 CLAVE
                Estado = true
            };
            nuevaCaja.Observaciones.Add(new ObservacionCaja
            {
                Texto = $"Caja abierta con saldo inicial de {saldoInicial}.",
                FechaCreacion = DateTime.UtcNow
            });

            await context.Cajas.AddAsync(nuevaCaja);
            await context.SaveChangesAsync();

            return nuevaCaja;
        }

        public async Task<IEnumerable<Caja>> ListarCajasCerradasAsync()
        {
            return await context.Cajas
                .Where(c => c.estadoCaja == Caja.EstadoCaja.Cerrada)
                .OrderByDescending(c => c.FechaFin)
                .Include(c => c.Movimientos)
                .Include(c => c.Observaciones)
                .ToListAsync();
        }

        public async Task<Caja> CambiarEstadoCajaAsync(int idCaja, Caja.EstadoCaja nuevoEstado, string? observacion = null)
        {
            try
            {
                var caja = await ObtenerCajaPorIdAsync(idCaja);

                ValidarTransicionEstado(caja, nuevoEstado);
                await ValidarRestriccionesGlobalesAsync(caja, nuevoEstado);

                // Asignar nuevo estado
                caja.estadoCaja = nuevoEstado;

                var texto = string.IsNullOrWhiteSpace(observacion)
                        ? $"Estado cambiado a {nuevoEstado}."
                        : observacion.Trim();

                caja.Observaciones.Add(new ObservacionCaja
                {
                    Texto = texto,
                    FechaCreacion = DateTime.UtcNow
                });

                if (nuevoEstado == Caja.EstadoCaja.Cerrada || nuevoEstado == Caja.EstadoCaja.Anulada)
                {
                    caja.FechaFin = DateTime.UtcNow; // cerrada o anulada → se marca la fecha de fin
                }
                else if (nuevoEstado == Caja.EstadoCaja.Abierta)
                {
                    caja.FechaFin = null; // opcional: limpiar si se vuelve a abrir (no debería pasar normalmente)
                }

                if (nuevoEstado == Caja.EstadoCaja.Anulada)
                {
                    caja.Estado = false; // desactivar caja si se anula

                    await context.ObservacionCajas.Where(o => o.CajaId == idCaja && o.Estado == true)
                        .ExecuteUpdateAsync(u => u.SetProperty(o => o.Estado, false));

                }
                await context.SaveChangesAsync();
                return caja;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }




        public async Task<decimal> CalcularSaldoFinalRealAsync(int cajaId)
        {
            var totalPagos = await context.Pagos
                .Where(p => p.CajaId == cajaId && p.Situacion == Situacion.Completo)
                .Include(p => p.Movimiento)
                .ThenInclude(m => m.TipoMovimiento)
                .ToListAsync();

            decimal saldo = 0;

            foreach (var pago in totalPagos)
            {
                if (pago.Movimiento.TipoMovimiento.signo == Signo.Suma)
                    saldo += pago.Monto;
                else
                    saldo -= pago.Monto;
            }

            var caja = await context.Cajas.AsNoTracking()
                .FirstAsync(c => c.Id == cajaId);

            return caja.SaldoInicial + saldo;
        }



        public async Task<decimal> CerrarCajaConSaldoFinalAsync(int cajaId, string? observacion = null)
        {
            // 1️⃣ Obtener caja
            var caja = await context.Cajas
                .Include(c => c.Movimientos)
                .FirstOrDefaultAsync(c => c.Id == cajaId);

            if (caja == null)
                throw new InvalidOperationException("La caja no existe.");

            if (caja.estadoCaja != Caja.EstadoCaja.Abierta)
                throw new InvalidOperationException("Solo se puede cerrar una caja abierta.");

            // 2️⃣ Verificar que no haya otra caja abierta (seguridad extra)
            bool existeOtraCajaAbierta = await context.Cajas
                .AnyAsync(c => c.estadoCaja == Caja.EstadoCaja.Abierta && c.Id != cajaId);

            if (existeOtraCajaAbierta)
                throw new InvalidOperationException("Existe otra caja abierta. No se puede cerrar esta.");

            // 3️⃣ Calcular saldo final real
            var saldoFinal = await CalcularSaldoFinalRealAsync(cajaId);

            // 4️⃣ Registrar observación automática
            var texto = string.IsNullOrWhiteSpace(observacion)
                ? $"Caja cerrada con saldo final real de {saldoFinal}."
                : observacion;

            caja.Observaciones.Add(new ObservacionCaja
            {
                Texto = texto,
                FechaCreacion = DateTime.UtcNow
            });

            // 5️⃣ Cerrar la caja usando tu lógica actual
            caja.estadoCaja = Caja.EstadoCaja.Cerrada;
            caja.FechaFin = DateTime.UtcNow;

            await context.SaveChangesAsync();

            // 6️⃣ Devolver el saldo final para la próxima caja
            return saldoFinal;
        }




        // ============================
        // MÉTODOS PRIVADOS AUXILIARES
        // ============================

        private async Task<Caja> ObtenerCajaPorIdAsync(int idCaja)
        {
            var caja = await context.Cajas.Include(c => c.Observaciones).FirstOrDefaultAsync(c => c.Id == idCaja);
            if (caja == null)
                throw new InvalidOperationException($"No se encontró la caja con ID {idCaja}.");

            return caja;
        }

        private async Task ValidarNoExisteCajaAbiertaAsync()
        {
            bool existeCajaAbierta = await context.Cajas.AnyAsync(c => c.estadoCaja == Caja.EstadoCaja.Abierta);
            if (existeCajaAbierta)
                throw new InvalidOperationException("Ya existe una caja abierta. Ciérrela antes de abrir otra.");
        }

        private static void ValidarTransicionEstado(Caja caja, Caja.EstadoCaja nuevoEstado)
        {
            // No permitir modificar una caja anulada
            if (caja.estadoCaja == Caja.EstadoCaja.Anulada)
                throw new InvalidOperationException("No se puede modificar una caja anulada.");

            // No permitir reabrir una caja cerrada
            if (caja.estadoCaja == Caja.EstadoCaja.Cerrada && nuevoEstado == Caja.EstadoCaja.Abierta)
                throw new InvalidOperationException("No se puede reabrir una caja cerrada.");
        }

        private async Task ValidarRestriccionesGlobalesAsync(Caja caja, Caja.EstadoCaja nuevoEstado)
        {
            // Evitar más de una caja abierta al mismo tiempo
            if (nuevoEstado == Caja.EstadoCaja.Abierta)
            {
                bool existeCajaAbierta = await context.Cajas.AnyAsync(c => c.estadoCaja == Caja.EstadoCaja.Abierta && c.Id != caja.Id);
                if (existeCajaAbierta)
                    throw new InvalidOperationException("Ya existe otra caja abierta. Cierre la actual antes de abrir otra.");
            }
        }
    }
}
