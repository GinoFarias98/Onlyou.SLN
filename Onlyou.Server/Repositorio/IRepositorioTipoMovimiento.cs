using Onlyou.BD.Data.Entidades;
using Onlyou.Shared.DTOS.TipoMovimento;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioTipoMovimiento : IRepositorio<TipoMovimiento>
    {
        Task EliminarTMovimientoAsync(int id);
        Task<List<TipoMovimiento>> FiltrarConRelacionesAsync(Dictionary<string, object?> filtros);
        Task ValidarNombreUnico(string nombre, int? id = null);
    }
}