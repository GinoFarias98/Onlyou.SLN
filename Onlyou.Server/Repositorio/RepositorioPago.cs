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

        public RepositorioPago(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<Pago>> SelectConRelaciones()
        {
            return await context.Pagos
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorMovimientoAsync(int movimientoId)
        {
            return await context.Pagos
                .Where(p => p.MovimientoId == movimientoId)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<Pago?> SelectConRelacionesXId(int id)
        {
            return await context.Pagos
                .Where(p => p.Id == id)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            return await context.Pagos
                .Where(p => p.FechaRealizado >= inicio && p.FechaRealizado <= fin)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorSituacionAsync(Situacion situacion)
        {
            return await context.Pagos
                .Where(p => p.Situacion == situacion)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pago>> SelectPagosPorMetodoAsync(int tipoPagoId)
        {
            return await context.Pagos
                .Where(p => p.TipoPagoId == tipoPagoId)
                .Include(p => p.Movimiento)
                .Include(p => p.TipoPago)
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<decimal> SelectTotalPagosPorMovimientoAsync(int movimientoId)
        {
            return await context.Pagos
                .Where(p => p.MovimientoId == movimientoId)
                .SumAsync(p => p.Monto);
        }

        public async Task<List<Pago>> SelectArchivadosConRelaciones()
        {
            return await context.Pagos
                .Where(p => !p.Estado)
                .Include(p => p.TipoPago)
                .Include(p => p.Movimiento)
                .Include(p => p.Observaciones)
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
                .Include(p => p.Observaciones)
                .ToListAsync();
        }

        public async Task<Pago> CambiarSituacionPagoAsync(int idPago, Situacion nuevaSituacion, string? observacion = null)
        {
            var pago = await context.Pagos
                .Include(p => p.Observaciones)
                .FirstOrDefaultAsync(p => p.Id == idPago);

            if (pago == null)
                throw new KeyNotFoundException($"No se encontró un Pago con ID {idPago}");

            ValidarTransicionSituacionPago(pago, nuevaSituacion);

            pago.Situacion = nuevaSituacion;

            pago.Estado = nuevaSituacion switch
            {
                Situacion.Anulado => false,
                Situacion.Completo => false,
                Situacion.Parcial => true,
                _ => pago.Estado
            };

            // Agregar observación como entidad
            if (!string.IsNullOrWhiteSpace(observacion))
            {
                pago.Observaciones.Add(new ObservacionPago
                {
                    FechaCreacion = DateTime.UtcNow,
                    Texto = observacion.Trim()
                });
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
