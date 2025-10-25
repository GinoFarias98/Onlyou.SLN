using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioMarca : Repositorio<Marca>, IRepositorioMarca
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioMarca(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Marca?> SelectByName(string name)
        {
            try
            {
                var marca = await context.Marcas.FirstOrDefaultAsync(m => m.Nombre == name);
                return marca;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<List<Marca>> SelectBySimilName(string similName)
        {
            try
            {
                var marcas = await context.Marcas.Where(m => EF.Functions.Like(m.Nombre, $"{similName.ToLower()}")).ToListAsync();
                return marcas;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<List<Producto>> SelectProdDesdeMarca(int id)
        {
            try
            {
                var productos = await context.Productos.Where(p => p.MarcaId == id).ToListAsync();
                return productos;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }


        public async Task EliminarMarcaAsync(int id)
        {
            var marca = await context.Marcas
                                         .Include(c => c.Productos)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (marca == null)
                throw new Exception("Marca no encontrada.");

            if (marca.Productos != null && marca.Productos.Any())
                throw new InvalidOperationException("No se puede eliminar una Marca con productos asociados.");

            context.Marcas.Remove(marca);
            await context.SaveChangesAsync();
        }

    }
}
