using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Services
{
    public interface ICajaService
    {
        Task<Caja> AbrirCajaAsync(decimal saldoInicial);
        Task<decimal> CerrarCajaAsync(int cajaId, string? obs);
    }
}