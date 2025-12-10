using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioObservacionPedido : Repositorio<ObservacionPedido>, IRepositorioObservacionPedido
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioObservacionPedido(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<ObservacionPedido> AgregarObservacionAsync(int pedidoId, string texto)
        {
            try
            {
                var obs = new ObservacionPedido
                {
                    PedidoId = pedidoId,
                    Texto = texto,
                    FechaCreacion = DateTime.UtcNow
                };

                context.ObservacionPedidos.Add(obs);
                await context.SaveChangesAsync();
                return obs;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<IEnumerable<ObservacionPedido>> ListarObservacionesAsync(int pedidoId)
        {
            try
            {
                var pedido = await context.Pedidos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == pedidoId);

                if (pedido is null)
                    throw new KeyNotFoundException($"No existe la Caja {pedidoId}");

                var query = context.ObservacionPedidos
                                   .Where(o => o.PedidoId == pedidoId);

                // Si la caja está activa, solo observaciones activas
                if (pedido.Estado)
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
