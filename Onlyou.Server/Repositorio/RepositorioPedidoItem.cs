using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioPedidoItem : Repositorio<PedidoItem>, IRepositorioPedidoItem
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioPedidoItem(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        // Obtener todos los ítems de un pedido
        public async Task<IEnumerable<PedidoItem>> SelectItemsPorPedidoAsync(int pedidoId)
        {
            try
            {
                return await context.PedidoItems
                    .Where(pi => pi.PedidoId == pedidoId)
                    .Include(pi => pi.Producto)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // Obtener un ítem por pedido y producto
        public async Task<PedidoItem?> SelectItemPorPedidoYProductoAsync(int pedidoId, int productoId)
        {
            try
            {
                return await context.PedidoItems
                    .FirstOrDefaultAsync(pi => pi.PedidoId == pedidoId && pi.ProductoId == productoId);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        //// Agregar un ítem al pedido
        //public async Task AgregarItemAsync(PedidoItem item)
        //{
        //    try
        //    {
        //        await context.PedidoItems.AddAsync(item);
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        ImprimirError(ex);
        //        throw;
        //    }
        //}

        //// Actualizar un ítem del pedido (cantidad, precio, etc.)
        //public async Task ActualizarItemAsync(PedidoItem item)
        //{
        //    try
        //    {
        //        context.PedidoItems.Update(item);
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        ImprimirError(ex);
        //        throw;
        //    }
        //}

        //// Eliminar un ítem del pedido
        //public async Task EliminarItemAsync(int pedidoItemId)
        //{
        //    try
        //    {
        //        var item = await context.PedidoItems.FindAsync(pedidoItemId);
        //        if (item != null)
        //        {
        //            context.PedidoItems.Remove(item);
        //            await context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ImprimirError(ex);
        //        throw;
        //    }
        //}

        // Calcular total del pedido sumando SubTotal de cada ítem
        public async Task<decimal> SelectTotalPedidoAsync(int pedidoId)
        {
            try
            {
                return await context.PedidoItems
                    .Where(pi => pi.PedidoId == pedidoId)
                    .SumAsync(pi => pi.PrecioUnitarioVenta * pi.Cantidad);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

    }
}
