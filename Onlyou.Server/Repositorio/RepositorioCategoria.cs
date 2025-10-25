using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioCategoria : Repositorio<Categoria>, IRepositorioCategoria
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioCategoria(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<Categoria>> SelecBySimilName(string similName)
        {
            try
            {
                var categorias = await context.Categorias.Where(c => EF.Functions.Like(c.Nombre, $"{similName.ToLower()}")).ToListAsync();
                return categorias;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<List<Producto>> SelectProdByCategoria(int id)
        {
            try
            {
                var productos = await context.Productos.Where(p => p.CategoriaId == id).ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


        public async Task EliminarCategoriaAsync(int id)
        {
            var categoria = await context.Categorias
                                         .Include(c => c.Productos)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
                throw new Exception("Categoría no encontrada.");

            if (categoria.Productos != null && categoria.Productos.Any())
                throw new InvalidOperationException("No se puede eliminar una categoría con productos asociados.");

            context.Categorias.Remove(categoria);
            await context.SaveChangesAsync();
        }


    }
}
