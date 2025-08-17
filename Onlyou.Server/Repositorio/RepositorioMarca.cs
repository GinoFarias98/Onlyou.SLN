using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioMarca : Repositorio<Marca>, IRepositorioMarca
    {
        private readonly Context context;

        public RepositorioMarca(Context context) : base(context)
        {
            this.context = context;
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
    }
}
