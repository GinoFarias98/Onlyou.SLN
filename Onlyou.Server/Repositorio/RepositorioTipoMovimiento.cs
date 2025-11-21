using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.TipoMovimento;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Onlyou.Server.Repositorio
{
    public class RepositorioTipoMovimiento : Repositorio<TipoMovimiento>, IRepositorioTipoMovimiento
    {
        private readonly Context context;
        private readonly IMapper mapper;

        public RepositorioTipoMovimiento(Context context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task ValidarNombreUnico(string nombre, int? id = null)
        {
            var existe = await context.TipoMovimientos
                .AnyAsync(x =>
                    x.Nombre.ToLower() == nombre.ToLower() &&
                    (id == null || x.Id != id)
                );

            if (existe)
                throw new Exception("Ya existe un tipo de movimiento con ese nombre.");
        }

        // ----------------------------
        // FILTRO USANDO EL SELECTOR GENÉRICO
        // ----------------------------
        public async Task<List<TipoMovimiento>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros)
        {
            try
            {
                var TipoMovimientosFiltrados = await FiltrarAsync(filtros);
                var ids = TipoMovimientosFiltrados.Select(p => p.Id).ToList();

                var TPFiltrados = await context.TipoMovimientos
                    .Where(p => ids.Contains(p.Id)) // Ojo: sin AsQueryable()
                    .ToListAsync();

                return TPFiltrados;

            }
            catch (Exception ex)
            {
                ImprimirError(ex);
                throw;
            }

        }



        public async Task EliminarTMovimientoAsync(int id)
        {
            var TipoMovimiento = await context.TipoMovimientos
                                         .Include(tm => tm.Movimientos)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (TipoMovimiento == null)
                throw new Exception("Categoría no encontrada.");

            if (TipoMovimiento.Movimientos != null && TipoMovimiento.Movimientos.Any())
                throw new InvalidOperationException("No se puede eliminar un Tipo de Movimiento con Movimientos asociados.");

            context.TipoMovimientos.Remove(TipoMovimiento);
            await context.SaveChangesAsync();
        }

    }
}
