using Onlyou.Shared.DTOS.Talle;

namespace Onlyou.Client.Servicios
{
    public interface ITalleServicios
    {
        Task<List<TallesDTO>> GetTalles();                // GET all
        Task<TallesDTO?> GetTallePorId(int id);           // GET by id
        Task<List<TallesDTO>> GetTallesPorProducto(int productoId); // GET por producto
        Task<int?> CrearTalle(TallesDTO talle);           // POST
        Task<bool> ActualizarTalle(int id, TallesDTO talle); // PUT
        Task<bool> EliminarTalle(int id);                 // DELETE
    }


    public class TalleServicios : ITalleServicios
    {
        private readonly IHttpServicios http;
        private readonly string urlBase = "api/talle";

        public TalleServicios(IHttpServicios http)
        {
            this.http = http;
        }

        public async Task<List<TallesDTO>> GetTalles()
        {
            var respuesta = await http.Get<List<TallesDTO>>(urlBase);
            return respuesta.Respuesta ?? new List<TallesDTO>();
        }

        public async Task<TallesDTO?> GetTallePorId(int id)
        {
            var respuesta = await http.Get<TallesDTO>($"{urlBase}/{id}");
            return respuesta.Respuesta;
        }

        public async Task<List<TallesDTO>> GetTallesPorProducto(int productoId)
        {
            var respuesta = await http.Get<List<TallesDTO>>($"{urlBase}/producto/{productoId}");
            return respuesta.Respuesta ?? new List<TallesDTO>();
        }

        public async Task<int?> CrearTalle(TallesDTO talle)
        {
            var respuesta = await http.Post(urlBase, talle);
            if (!respuesta.Error)
            {
                return int.Parse(respuesta.Respuesta?.ToString() ?? "0");
            }
            return null;
        }

        public async Task<bool> ActualizarTalle(int id, TallesDTO talle)
        {
            var respuesta = await http.Put($"{urlBase}/{id}", talle);
            return !respuesta.Error;
        }

        public async Task<bool> EliminarTalle(int id)
        {
            var respuesta = await http.Delete($"{urlBase}/{id}");
            return !respuesta.Error;
        }
    }
}
