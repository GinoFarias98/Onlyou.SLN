using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioCaja
    {
        Task<Caja?> SelectCajaAbiertaAsync();
        Task<IEnumerable<Caja>> SelectCajasPorRangoFechasAsync(DateTime inicio, DateTime fin);
    }
}