using Onlyou.BD.Data.Entidades;

namespace Onlyou.Server.Services
{
    public interface IPagoService
    {
        Task<Pago> CambiarSituacionAsync(int idPago, Situacion situacion, string? obs);
        Task<Pago> RegistrarPagoAsync(Pago pago);
    }
}