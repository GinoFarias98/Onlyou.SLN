using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioPedido : Repositorio<Pedido>, IRepositorioPedido
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioPedido(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }




        // Obtener pedido por ID incluyendo items y movimientos
        public async Task<Pedido?> SelectPedidoPorIdAsync(int pedidoId)
        {
            try
            {
                return await context.Pedidos
                    .Include(p => p.EstadoPedido) // ← AGREGAR ESTO
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Producto) // ← AGREGAR ESTO
                        .ThenInclude(producto => producto.Proveedor)
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
                var query = context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .Where(p => p.NombreCliente.Contains(nombreCliente));

                if (dni.HasValue)
                    query = query.Where(p => p.DNI == dni.Value);

                return await query.ToListAsync();
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
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                    .Include(p => p.Movimientos)
                    .Where(p => p.FechaGenerado >= inicio && p.FechaGenerado <= fin)
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

        public async Task<IEnumerable<Pedido>> GetAllWithDetailsAsync()
        {
            return await context.Pedidos
                   .Include(p => p.EstadoPedido)
                   .Include(p => p.PedidoItems)
                       .ThenInclude(pi => pi.Producto)
                           .ThenInclude(producto => producto.Proveedor)
                   .Include(p => p.PedidoItems)
                       .ThenInclude(pi => pi.Color)
                   //.Include(p => p.PedidoItems)
                   //    .ThenInclude(pi => pi.ColorNombre)

                   .Include(p => p.PedidoItems)
                       .ThenInclude(pi => pi.Talle)
                   //.Include(p => p.PedidoItems)
                   //    .ThenInclude(pi => pi.TalleNombre)

                   .OrderByDescending(p => p.FechaGenerado)
                   .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await context.Pedidos.AnyAsync(p => p.Id == id);
        }

        // NUEVO MÉTODO PARA ACTUALIZAR
        public async Task UpdateAsync(Pedido entidad)
        {
            try
            {
                context.Pedidos.Update(entidad);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        private void ImprimirError(Exception ex)
        {
            Console.WriteLine($"Error en RepositorioPedido: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
        }

    }
}

