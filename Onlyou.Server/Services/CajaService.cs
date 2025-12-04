using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;

namespace Onlyou.Server.Services
{
    public class CajaService : ICajaService
    {
        private readonly IRepositorioCaja repoCaja;

        public CajaService(IRepositorioCaja repoCaja)
        {
            this.repoCaja = repoCaja;
        }

        public async Task<Caja> AbrirCajaAsync(decimal saldoInicial)
        {
            return await repoCaja.AbrirNuevaCajaAsync(saldoInicial);
        }

        public async Task<decimal> CerrarCajaAsync(int cajaId, string? obs)
        {
            return await repoCaja.CerrarCajaConSaldoFinalAsync(cajaId, obs);
        }
    }
}
