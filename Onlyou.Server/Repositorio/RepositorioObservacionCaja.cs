using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioObservacionCaja : Repositorio<ObservacionCaja>, IRepositorioObservacionCaja
    {
        private readonly Context context;

        public RepositorioObservacionCaja(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
        }

        public async Task<ObservacionCaja> AgregarObservacionAsync(int cajaId, string texto)
        {
            var obs = new ObservacionCaja
            {
                CajaId = cajaId,
                Texto = texto,
                FechaCreacion = DateTime.UtcNow
            };

            context.ObservacionCajas.Add(obs);
            await context.SaveChangesAsync();
            return obs;

        }

        public async Task<IEnumerable<ObservacionCaja>> ListarObservacionesAsync(int cajaId)
        {
            return await context.ObservacionCajas.Where(o => o.CajaId == cajaId)
                                                 .OrderBy(o => o.FechaCreacion)
                                                 .ToListAsync();
        }

    }
}
