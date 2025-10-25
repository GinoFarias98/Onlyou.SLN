using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.Server.Utils;
using System.Linq.Expressions;

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
        private readonly IMapper mapper;

        public Repositorio(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Set<E>().AnyAsync(x => x.Id == id);
        }

        public async Task<bool> ExistePorCondicion(Expression<Func<E, bool>> predicado)
        {
            try
            {
                return await context.Set<E>().AnyAsync(predicado);
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


        public async Task<List<E>> Select()
        {
            try
            {
                //solo traemos los que tengan estado activo
                return await context.Set<E>().Where(x => x.Estado == true).ToListAsync();
                               
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw; // lanzamos excepcion hascia una capa superior para manejarlo
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


        public async Task<List<E>> SelectPorCondicion(Expression<Func<E, bool>> predicado, bool soloActivos = true)
        {
            try
            {
                IQueryable<E> query = context.Set<E>();

                // si la entidad tiene prop "Estado" y se puidio solo activos, filtra
                if (soloActivos)
                {
                    var propiedad = typeof(E).GetProperty("Estado");
                    if (propiedad != null && propiedad.PropertyType == typeof(bool))
                    {
                        query = query.Where(e => EF.Property<bool>(e, "Estado") == true);
                    }
                }

                // aplicamos la cond adicional

                query = query.Where(predicado);

                return await query.ToListAsync();

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }

        public async Task<List<E>> SelectArchivados()
        {
            try
            {
                return await context.Set<E>().Where(x => x.Estado == false).ToListAsync();
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
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

        public virtual async Task<TDTO> InsertDevuelveDTO<TDTO>(E entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula.");

            }

            try
            {
                await context.AddAsync(entidad);
                await context.SaveChangesAsync();

                var dto = mapper.Map<TDTO>(entidad);
                return dto;
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

        public async Task<bool> UpdateEstado(int id)
        {
            try
            {
                var entidadSelect = await context.Set<E>().FirstOrDefaultAsync(e => e.Id == id);
                if (entidadSelect == null)
                    return false;

                var propiedad = typeof(E).GetProperty("Estado");
                if (propiedad == null || propiedad.PropertyType != typeof(bool))
                    throw new InvalidOperationException("La entidad no contiene una propiedad 'Estado'.");

                bool estadoActual = (bool)propiedad.GetValue(entidadSelect)!;
                propiedad.SetValue(entidadSelect, !estadoActual);

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
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
