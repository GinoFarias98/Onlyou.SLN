using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioObservacionPago : Repositorio<ObservacionPago>, IRepositorioObservacionPago
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioObservacionPago(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ObservacionPago> AgregarObservacionAsync(int pagoID, string texto)
        {
            try
            {
                var obs = new ObservacionPago
                {
                    PagoId = pagoID,
                    Texto = texto,
                    FechaCreacion = DateTime.UtcNow
                };

                context.ObservacionPagos.Add(obs);
                await context.SaveChangesAsync();
                return obs;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<IEnumerable<ObservacionPago>> ListarObservacionesAsync(int pagoID)
        {
            try
            {
                var pago = await context.Pagos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == pagoID);

                if (pago is null)
                    throw new KeyNotFoundException($"No existe el Pago {pagoID}");

                var query = context.ObservacionPagos
                                   .Where(o => o.PagoId == pagoID);

                // Si la caja está activa, solo observaciones activas
                if (pago.Estado)
                {
                    query = query.Where(o => o.Estado == true);
                }

                // Si está anulada → traigo todas las observaciones
                // Incluye la que explica la anulación

                return await query
                    .OrderBy(o => o.FechaCreacion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


    }
}
