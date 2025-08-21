using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioEstadoPedido : Repositorio<EstadoPedido>, IRepositorioEstadoPedido
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioEstadoPedido(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<EstadoPedido>> SelectBySimilName(string similName)
        {
            try
            {
                var estadoPedido = await context.EstadoPedidos.Where(ep => EF.Functions.Like(ep.Nombre, $"{similName.ToLower()}")).ToListAsync();
                return estadoPedido;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<List<Pedido>> SelectProductoByEstadoPedido(int id)
        {
            try
            {
                var pedidos = await context.Pedidos.Where(p => p.EstadoPedidoId == id).ToListAsync();
                return pedidos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

    }
}
