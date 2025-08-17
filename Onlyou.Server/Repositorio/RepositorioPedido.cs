using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioPedido : Repositorio<Pedido>, IRepositorioPedido
    {
        private readonly Context context;
        public RepositorioPedido(Context context) : base(context)
        {
            this.context = context;
        }

        // Obtener pedido por ID incluyendo items y movimientos ------no hace falta mepa
        public async Task<Pedido?> SelectPedidoPorIdAsync(int pedidoId)
        {
            try
            {
                return await context.Pedidos
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .FirstOrDefaultAsync(p => p.Id == pedidoId);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener pedidos por cliente
        public async Task<IEnumerable<Pedido>> SelectPedidosPorClienteAsync(string nombreCliente, int? dni = null)
        {
            try
            {
                var query = context.Pedidos.AsQueryable();

                query = query.Where(p => p.NombreCliente.Contains(nombreCliente));

                if (dni.HasValue)
                    query = query.Where(p => p.DNI == dni.Value);

                return await query
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        //Obtener pedidos por estado de pago
        public async Task<IEnumerable<Pedido>> SelectPedidosPorEstadoPagoAsync(Pedido.EstadoPago estadoPago)
        {
            try
            {
                return await context.Pedidos
                    .Where(p => p.EstadoPago == estadoPago)
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        Obtener pedidos por estado de entrega
        public async Task<IEnumerable<Pedido>> SelectPedidosPorEstadoEntregaAsync(Pedido.EstadoEntrega estadoEntrega)
        {
            try
            {
                return await context.Pedidos
                    .Where(p => p.EstadoEntrega == estadoEntrega)
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener pedidos por rango de fechas
        public async Task<IEnumerable<Pedido>> SelectPedidosPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                return await context.Pedidos
                    .Where(p => p.FechaGenerado >= inicio && p.FechaGenerado <= fin)
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Total de saldo pendiente de todos los pedidos
        public async Task<decimal> SelectSaldoPendienteTotalAsync()
        {
            try
            {
                return await context.Pedidos
                    .SumAsync(p => p.MontoTotal - p.MontoPagado);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }
    }
}
