using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioCaja : Repositorio<Caja>, IRepositorioCaja
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioCaja(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Caja?> SelectCajaAbiertaAsync()
        {
            try
            {
                Caja? cajaAbiertas = await context.Cajas
               .Include(c => c.Movimientos)
               .FirstOrDefaultAsync(c => c.EstadoCaja == "Abierta");
                return cajaAbiertas;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }

        public async Task<IEnumerable<Caja>> SelectCajasPorRangoFechasAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                var cajasPorFecha = await context.Cajas
                    .Where(c => c.FechaInicio <= fin && c.FechaFin >= inicio)
                    .Include(c => c.Movimientos)
                    .ToListAsync();

                return cajasPorFecha;
            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }
        }


    }

}
