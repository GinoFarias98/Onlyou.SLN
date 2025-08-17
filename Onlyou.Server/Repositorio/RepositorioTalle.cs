using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTalle : Repositorio<Talle>, IRepositorioTalle
    {
        private readonly Context context;

        public RepositorioTalle(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Talle>> SelectTallePorProducto(int productoId)
        {
            try
            {
                var producto = await context.Productos.Include(p => p.ProductosTalles)
                        .ThenInclude(pt => pt.Talle)
                        .FirstOrDefaultAsync(p => p.Id == productoId);

                var TallesDelProducto = producto?.ProductosTalles.Select(pt => pt.Talle).ToList();
                return TallesDelProducto;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

    }
}
