using Onlyou.Shared.DTOS.Producto;

namespace Onlyou.Client.Servicios.ServiciosEntidades
{
    public class ProductoServicio
    {
        private readonly FiltroGenericoServicio<GetProductoDTO> filtroServicio;

        public ProductoServicio(HttpServicios http)
        {
            filtroServicio = new FiltroGenericoServicio<GetProductoDTO>(http, "productos");
        }

        public async Task<HttpRespuesta<List<GetProductoDTO>>> FiltrarAsync(Dictionary<string, object> filtros)
        {
            return await filtroServicio.FiltrarAsync(filtros);
        }
    }
}