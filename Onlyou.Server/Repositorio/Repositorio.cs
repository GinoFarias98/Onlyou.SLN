using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;

namespace Onlyou.Server.Repositorio
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntidadBase
    {
        


        private Context context;

        public Repositorio(Context context)
        {
            this.context = context;
        }

        public async Task<bool> Existe(int id)
        {
            var existe = await context.Set<E>().AnyAsync(e => e.Id == id);
            return existe;
        }

        public async Task<bool> ExisteByCodigo(string codigo)
        {
            var existeByCodigo = await context.Set<E>().AnyAsync(e => e.Codigo == codigo);
            return existeByCodigo;
        }

        public async Task<List<E>> Select()
        {
            try
            {
                return await context.Set<E>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro al obtener los registros: {ex.Message}");
                throw; // lanzamos excepcion hascia una capa superior para manejarlo
            }
        }

        //public async Task<E?> SelectByCod(string codigo)
        //{
        //    try
        //    {
        //        if (await ExisteByCodigo(codigo))
        //        {
        //            E? EntidadByCodigo = await context.Set<E>().FirstOrDefaultAsync(e => e.Codigo == codigo);
        //            return EntidadByCodigo;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al{ex}");
        //        throw;
        //    }
        //}

    }
}
