using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.Server.Utils;

namespace Onlyou.Server.Repositorio
{
    public class Repositorio<E> : IRepositorio<E> where E : class, IEntidadBase
    {

        protected static void ImprimirError(Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {DateTime.Now}: {exception.Message}");
            Console.WriteLine(exception.StackTrace);
            Console.ResetColor();
        }

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
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw; // lanzamos excepcion hascia una capa superior para manejarlo
            }
        }

        public async Task<E?> SelectByCod(string codigo)
        {
            try
            {
                if (!await ExisteByCodigo(codigo))
                {
                    Console.WriteLine($"El codigo {codigo} No existe");
                    return null;
                }

                return await context.Set<E>().FirstOrDefaultAsync(e => e.Codigo == codigo); ;


            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }
        }

        public async Task<E?> SelectById(int id)
        {
            try
            {
                if (!await Existe(id))
                {
                    Console.WriteLine($"El codigo {id} No existe");
                    return null;
                }

                return await context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);


            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }
        }

        public async Task<int> Insert(E entidad)
        {
            try
            {
                await context.Set<E>().AddAsync(entidad);
                await context.SaveChangesAsync();
                return entidad.Id;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }
        }

        public async Task<string?> InsertDevuelveCodigo(E entidad)
        {
            try
            {
                await context.Set<E>().AddAsync(entidad);
                await context.SaveChangesAsync();
                return entidad.Codigo;
            }

            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }
        }

        public async Task<bool> UpdateEntidad(int id, E entidad)
        {
            if (id != entidad.Id)
            {
                return false;
            }

            var EntidadExiste = await SelectById(id);

            if (EntidadExiste == null)
            {
                return false;
            }


            try
            {
                context.Entry(EntidadExiste).CurrentValues.SetValues(entidad);
                //El metodo de arriba toma los valores de la entidad seleccionada por id (EntidadExistente)
                //y los actualiza con los de la entidad pasada como argumento (entidad).

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }

        }

        public async Task<bool> UpdateEstado(int id, E entidad)
        {

            try
            {
                var entidadSelect = await SelectById(id);
                if (entidadSelect == null)
                {
                    return false;
                }

                var propiedad = typeof(E).GetProperty("Estado");

                if (propiedad == null || propiedad.PropertyType != typeof(bool))
                {
                    throw new InvalidOperationException("La entidad No contiende una propiedad 'Estado'");
                }

                bool estadoActual = (bool)propiedad.GetValue(entidad)!;
                propiedad.SetValue(entidad, !estadoActual);

                context.Update(entidad);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }

        }


        public async Task<bool> Delete(int id)
        {
            var EntidadSeleccionada = await SelectById(id);
            if (EntidadSeleccionada == null)
            {
                return false;
            }

            context.Set<E>().Remove(EntidadSeleccionada);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
