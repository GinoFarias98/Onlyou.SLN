using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Talle;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTalle : Repositorio<Talle>, IRepositorioTalle
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioTalle(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        //public async Task<IEnumerable<Talle>> Select()
        //{
        //    return await context.Talles
        //        .Select(t => new Talle
        //        {
        //            Id = t.Id,
        //            Nombre = t.Nombre ?? "" // <- si es NULL, devuelve string vacío
        //        })
        //        .ToListAsync();
        //}


        public override async Task<TDTO> InsertDevuelveDTO<TDTO>(Talle entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula");
            }

            try
            {
                // verificamos que no este duplicado
                bool existe = await context.Talles.AnyAsync(t => t.Nombre.ToLower() == entidad.Nombre.ToLower());
                if (existe) 
                {
                    throw new InvalidOperationException($"El Talle '{entidad.Nombre}' ya existe en el sistema");
                }

                await context.Talles.AddAsync(entidad);
                await context.SaveChangesAsync();
                
                var dto = mapper.Map<TDTO>(entidad);
                return dto;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
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
