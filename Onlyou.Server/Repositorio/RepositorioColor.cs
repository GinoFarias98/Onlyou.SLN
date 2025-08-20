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

        public RepositorioColor(Context context,IMapper mapper) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
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

        public async Task<GetColorDTO> InsertDevuelveDTO(Color color)
        {
            try
            {

                await context.AddAsync(color);
                await context.SaveChangesAsync();

                var colorDTODevuelto = mapper.Map<GetColorDTO>(color);

                return colorDTODevuelto;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                //Descomentar al Publicar el proyecto en IIS
                //Logger.LogError(ex);
                throw;
            }
        }
    }
   
}
