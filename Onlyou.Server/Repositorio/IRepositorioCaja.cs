using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Repositorio
{
    public interface IRepositorioCaja : IRepositorio<Caja>
    {
        Task<Caja> AbrirNuevaCajaAsync(decimal saldoInicial);
        Task<decimal> CalcularSaldoFinalRealAsync(int cajaId);
        Task<Caja> CambiarEstadoCajaAsync(int idCaja, Caja.EstadoCaja nuevoEstado, string? observacion = null);
        Task<decimal> CerrarCajaConSaldoFinalAsync(int cajaId, string? observacion = null);
        Task<IEnumerable<Caja>> ListarCajasCerradasAsync();
        Task<Caja> RecalcularSaldoAsync(int cajaId);
        Task<Caja?> SelectCajaAbiertaAsync();
        Task<IEnumerable<Caja>> SelectCajasPorRangoFechasAsync(DateTime inicio, DateTime fin);
        Task ValidarCajaHabilitadaParaOperar(int cajaId);
    }
}