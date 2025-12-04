using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;

namespace Onlyou.Server.Services
{
    public class PagoService : IPagoService
    {
        private readonly IRepositorioPago repoPago;
        private readonly IRepositorioMovimiento repoMovimiento;

        public PagoService(
            IRepositorioPago repoPago,
            IRepositorioMovimiento repoMovimiento)
        {
            this.repoPago = repoPago;
            this.repoMovimiento = repoMovimiento;
        }

        public async Task<Pago> RegistrarPagoAsync(Pago pago)
        {
            // Esto encapsula toda tu lógica actual
            return await repoPago.RegistrarPagoConImpactoAsync(pago);
        }

        public async Task<Pago> CambiarSituacionAsync(int idPago, Situacion situacion, string? obs)
        {
            var pago = await repoPago.CambiarSituacionPagoAsync(idPago, situacion, obs);

            // 🔁 Recalcular movimiento del pago
            await repoMovimiento.RecalcularEstadoMovimientoPorPagosAsync(pago.MovimientoId);

            return pago;
        }
    }
}
