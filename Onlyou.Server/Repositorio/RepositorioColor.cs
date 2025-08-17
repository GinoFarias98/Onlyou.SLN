using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Color = Onlyou.BD.Data.Entidades.Color;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioColor : Repositorio<Color>, IRepositorioColor
    {
        private readonly Context context;

        public RepositorioColor(Context context) : base(context)
        {
            this.context = context;
        }

        public async Task<string?> ObtenerHexa(int id)
        {
            try
            {
                var hexa = await context.Colores.FirstOrDefaultAsync(c => c.Id == id);
                return hexa?.Hexadecimal;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

    }
}
