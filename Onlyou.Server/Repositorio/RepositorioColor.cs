using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.Color;
using Color = Onlyou.BD.Data.Entidades.Color;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioColor : Repositorio<Color>, IRepositorioColor
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioColor(Context context,IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public override async Task<TDTO> InsertDevuelveDTO<TDTO>(Color entidad)
        {
            if (entidad == null)
            {
                throw new ArgumentNullException(nameof(entidad), "La entidad no puede ser nula");
            }

            try
            {
                // verificamos que no este duplicado
                bool existe = await context.Colores.AnyAsync(t => t.Nombre.ToLower() == entidad.Nombre.ToLower());
                if (existe)
                {
                    throw new InvalidOperationException($"El Color '{entidad.Nombre}' ya existe en el sistema");
                }

                await context.Colores.AddAsync(entidad);
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

        public async Task<string?> ObtenerHexa(int id)
        {
            try
            {
                var color = await context.Colores.FirstOrDefaultAsync(c => c.Id == id);
                return color?.Hexadecimal;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        //public async Task<GetColorDTO> InsertDevuelveDTO(Color color)
        //{
        //    try
        //    {

        //        await context.AddAsync(color);
        //        await context.SaveChangesAsync();

        //        var colorDTODevuelto = mapper.Map<GetColorDTO>(color);

        //        return colorDTODevuelto;
        //    }
        //    catch (Exception ex)
        //    {
        //        ImprimirError(ex);
        //        //Descomentar al Publicar el proyecto en IIS
        //        //Logger.LogError(ex);
        //        throw;
        //    }
        //}
    }
   
}
