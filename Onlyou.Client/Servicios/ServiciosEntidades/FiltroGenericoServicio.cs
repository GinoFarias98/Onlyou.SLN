namespace Onlyou.Client.Servicios.ServiciosEntidades
{
    public class FiltroGenericoServicio<T>
    {
        private readonly IHttpServicios http;
        private readonly string urlBase;

        public FiltroGenericoServicio(IHttpServicios http, string urlBase)
        {
            this.http = http;
            this.urlBase = urlBase;
        }

        /// <summary>
        /// Envía filtros genéricos al backend y devuelve una lista de resultados del tipo T.
        /// </summary>
        public async Task<HttpRespuesta<List<T>>> FiltrarAsync(Dictionary<string, object> filtros)
        {
            var url = $"{urlBase}/filtrar"; // Ej: api/productos/filtrar
            return await http.Post<Dictionary<string, object>, List<T>>(url, filtros);
        }
    }
}