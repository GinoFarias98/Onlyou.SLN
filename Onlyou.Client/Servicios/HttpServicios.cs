using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Onlyou.Client.Servicios
{
    public class HttpServicios : IHttpServicios
    {
        private readonly HttpClient http;

        public HttpServicios(HttpClient http)
        {
            this.http = http;
        }

        private async Task<O?> DesSerealizar<O>(HttpResponseMessage response)
        {
            var respuesta = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<O>(respuesta, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<HttpRespuesta<O>> Get<O>(string url)
        {
            var response = await http.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerealizar<O>(response);
                return new HttpRespuesta<O>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<O>(default, true, response);
            }
        }


        public async Task<HttpRespuesta<object>> Post<O>(string url, O entidad)  // "O" lo que se envia, "object" lo que recibo.
        {
            var EntSerializada = JsonSerializer.Serialize(entidad);
            var EnviarJSON = new StringContent(EntSerializada, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, EnviarJSON);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerealizar<object>(response);
                return new HttpRespuesta<object>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<object>(default, true, response);
            }
        }

        public async Task<HttpRespuesta<object>> Put<O>(string url, O entidad)
        {
            var EntSerializada = JsonSerializer.Serialize(entidad);
            var EnviarJSON = new StringContent(EntSerializada, Encoding.UTF8, "application/json");
            var response = await http.PutAsync(url, EnviarJSON);
            if (response.IsSuccessStatusCode)
            {
                return new HttpRespuesta<object>(null, false, response);
            }
            else
            {
                return new HttpRespuesta<object>(default, true, response);
            }
        }



        #region Sobreescritura Metodos Put y Post

        public async Task<HttpRespuesta<TResponse>> Post<TRequest, TResponse>(string url, TRequest entidad)
        {
            var EntSerializada = JsonSerializer.Serialize(entidad);
            var EnviarJSON = new StringContent(EntSerializada, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, EnviarJSON);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerealizar<TResponse>(response);
                return new HttpRespuesta<TResponse>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<TResponse>(default!, true, response);
            }
        }

        public async Task<HttpRespuesta<TResponse>> Put<TRequest, TResponse>(string url, TRequest entidad)
        {
            var EntSerializada = JsonSerializer.Serialize(entidad);
            var EnviarJSON = new StringContent(EntSerializada, Encoding.UTF8, "application/json");
            var response = await http.PutAsync(url, EnviarJSON);

            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerealizar<TResponse>(response);
                return new HttpRespuesta<TResponse>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<TResponse>(default!, true, response);
            }
        }


        #endregion


        public async Task<HttpRespuesta<object>> Delete(string url)
        {
            var response = await http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return new HttpRespuesta<object>(null, false, response);
            }
            else
            {
                return new HttpRespuesta<object>(default, true, response);
            }
        }
    }
}

