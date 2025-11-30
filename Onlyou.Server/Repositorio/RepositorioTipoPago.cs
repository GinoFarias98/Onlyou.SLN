using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTipoPago : Repositorio<TipoPago>, IRepositorioTipoPago
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioTipoPago(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<TipoPago> SelectTipoPagosxId(int idTipoPago)
        {
            try
            {
                if (idTipoPago <= 0)
                {
                    throw new ArgumentException($"El ID del Tipo de Pago debe ser mayor que cero. ID recibido: {idTipoPago}");
                }

                var tipoPago = await context.TipoPagos.Where(p => p.Id == idTipoPago).FirstOrDefaultAsync();

                if (tipoPago == null)
                {
                    throw new KeyNotFoundException($"No se encontró un Tipo Pago con el ID {idTipoPago}.");
                }

                return tipoPago;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }



        public async Task<List<Pago>> SelectPagoDesdeTipoPago(int id)
        {
            try
            {
                var pagos = await context.Pagos.Where(p => p.TipoPagoId == id).ToListAsync();
                return pagos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }


        public async Task EliminarTipoPagoAsync(int id)
        {
            var tipoPago = await context.TipoPagos
                                         .Include(c => c.Pagos)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (tipoPago == null)
                throw new Exception("Tipo de Pago no encontrada.");

            if (tipoPago.Pagos != null && tipoPago.Pagos.Any())
                throw new InvalidOperationException("No se puede eliminar un Tipo de Pago con Pago asociados.");

            context.TipoPagos.Remove(tipoPago);
            await context.SaveChangesAsync();
        }


    }

}
