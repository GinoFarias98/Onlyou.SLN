using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTipoProducto : Repositorio<TipoProducto>, IRepositorioTipoProducto
    {
        private readonly Context context;

        public RepositorioTipoProducto(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<TipoProducto>> SelectBySimilName(string similName)
        {
            try
            {
                var tipoProductos = await context.TipoProductos.Where(tp => EF.Functions.Like(tp.Nombre, $"{similName.ToLower()}")).ToListAsync();
                return tipoProductos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<List<Producto>> SelectProductosByTipoProd(int id)
        {
            try
            {
                var productos = await context.Productos.Where(p => p.TipoProductoId == id).ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

    }
}
