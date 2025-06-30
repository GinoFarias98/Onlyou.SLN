
namespace Onlyou.Client.Servicios
{
    public interface IHttpServicios
    {
        Task<HttpRespuesta<object>> Delete(string url);
        Task<HttpRespuesta<O>> Get<O>(string url);
        Task<HttpRespuesta<object>> Post<O>(string url, O entidad);
        Task<HttpRespuesta<object>> Put<O>(string url, O entidad);
    }
}