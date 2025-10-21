using System.Net;
using System.Text.Json;

namespace Onlyou.Client.Servicios
{
    public class HttpRespuesta<T>
    {
        public T? Respuesta { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponseMessage { get; }

        public HttpRespuesta(T? respuesta, bool error, HttpResponseMessage httpResponseMessage)
        {
            Respuesta = respuesta;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string> ObtenerErrorAsync()
        {
            if (!Error || HttpResponseMessage == null)
                return string.Empty;

            try
            {
                var statusCode = HttpResponseMessage.StatusCode;
                var contenido = await HttpResponseMessage.Content.ReadAsStringAsync();

                // Intentamos parsear si el backend devuelve JSON { "mensaje": "..." }
                string? mensaje = null;
                if (!string.IsNullOrWhiteSpace(contenido))
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(contenido);
                        if (doc.RootElement.TryGetProperty("mensaje", out var prop))
                        {
                            mensaje = prop.GetString();
                        }
                        else
                        {
                            // Si no hay campo "mensaje", devolvemos el texto crudo
                            mensaje = contenido;
                        }
                    }
                    catch
                    {
                        // No era JSON, devolvemos texto plano
                        mensaje = contenido;
                    }
                }

                // Devolvemos mensajes más amigables según código HTTP
                return statusCode switch
                {
                    HttpStatusCode.BadRequest => mensaje ?? "Solicitud inválida.",
                    HttpStatusCode.Unauthorized => "No está autenticado.",
                    HttpStatusCode.Forbidden => "No tiene permisos para ejecutar esta acción.",
                    HttpStatusCode.NotFound => "Recurso no encontrado.",
                    HttpStatusCode.Conflict => mensaje ?? "El recurso ya existe o hay un conflicto.",
                    HttpStatusCode.InternalServerError => "Error interno del servidor.",
                    _ => mensaje ?? $"Error desconocido ({statusCode})."
                };
            }
            catch (Exception ex)
            {
                return $"Error al procesar la respuesta del servidor: {ex.Message}";
            }
        }
    }
}
