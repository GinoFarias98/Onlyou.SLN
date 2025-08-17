using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioProducto : Repositorio<Producto>, IRepositorioProducto
    {
        private readonly Context context;

        public RepositorioProducto(Context context) : base(context)
        {
            this.context = context;
        }


        public async Task<List<Talle>> SelectTallesDisponibles(int productoId)
        {
            try
            {
                var producto = await context.Productos.Include(pt => pt.ProductosTalles).ThenInclude(pt => pt.Talle)
                .FirstOrDefaultAsync(p => p.Id == productoId);

                if (producto == null)
                {
                    return new List<Talle>();
                    // IMPORTANTE: manejar desde el controller que si viene
                    // una lista vacia, no existe el producto
                }

                return producto.ProductosTalles.Select(pt => pt.Talle).ToList();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        // metodo que busca por nombre paracial y devuelve similares a lo que se escribe
        public async Task<List<Producto>> SelectBySimilName(string similName)
        {
            try
            {
                // busqueda case-insensitive
                var productos = await context.Productos.Where(p => EF.Functions.Like(p.Nombre, $"%{similName.ToLower()}")).ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);

                throw;
            }

        }

        public async Task<List<Producto>> SelectByTipoProducto(int tipoProductoId)
        {
            try
            {
                var productos = await context.Productos.Where(p => p.TipoProductoId == tipoProductoId).ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<Producto?> SelectConRelaciones(int id)
        {
            try
            {
                var productoCompleto = await context.Productos.Include(p => p.ProductosTalles).ThenInclude(pt => pt.Talle)
                    .Include(p => p.ProductosColores).ThenInclude(pc => pc.Color)
                    .Include(p => p.Marca)
                    .Include(p => p.Categoria)
                    .Include(p => p.TipoProducto)
                    .FirstOrDefaultAsync(p => p.Id == id);

                return productoCompleto;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        // para paginacion
        // skip = cantidad de productos omitidos
        // take = cantidad de productos por pagina

        public async Task<List<Producto>> SelectPaginado(int skip, int take)
        {
            var productosPaginados = await context.Productos.OrderBy(p => p.Nombre)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            return productosPaginados;
        }

        public async Task<List<Producto>> SelectProductoByProv(int proveedorId)
        {
            try
            {
                var productoByProv = await context.Productos.Where(prov => prov.Id == proveedorId).ToListAsync();
                return productoByProv;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<List<Producto>> SelectProdByMarcaFiltro(int marcaId)
        {
            try
            {
                var productosByMarca = await context.Productos.Where(p => p.MarcaId == marcaId).ToListAsync();
                return productosByMarca;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

    }
}
