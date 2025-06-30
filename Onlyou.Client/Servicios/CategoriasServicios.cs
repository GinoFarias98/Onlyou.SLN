using Onlyou.Client.Servicios;
using Onlyou.Shared.DTOS;
using Onlyou.Shared.DTOS.Categorias;
using System.Net.Http.Json;

namespace Onlyou.Client.Servicios
{
    public interface ICategoriaServicios
    {
        Task<List<GetCategoriasDTO>> GetCategorias();
        Task<int?> CrearCategoria(CrearCategoriasDTO categoria);
        Task<bool> ActualizarCategoria(int id, CrearCategoriasDTO categoria);
        Task<bool> EliminarCategoria(int id);
    }

    public class CategoriaServicios : ICategoriaServicios
    {
        private readonly IHttpServicios http;
        private readonly string urlBase = "api/categorias";

        public CategoriaServicios(IHttpServicios http)
        {
            this.http = http;
        }

        public async Task<List<GetCategoriasDTO>> GetCategorias()
        {
            var respuesta = await http.Get<List<GetCategoriasDTO>>(urlBase);
            return respuesta.Respuesta!;
        }

        public async Task<int?> CrearCategoria(CrearCategoriasDTO categoria)
        {
            var respuesta = await http.Post(urlBase, categoria);
            if (!respuesta.Error)
            {
                return int.Parse(respuesta.Respuesta?.ToString() ?? "0");
            }
            return null;
        }

        public async Task<bool> ActualizarCategoria(int id, CrearCategoriasDTO categoria)
        {
            var respuesta = await http.Put($"{urlBase}/{id}", categoria);
            return !respuesta.Error;
        }

        public async Task<bool> EliminarCategoria(int id)
        {
            var respuesta = await http.Delete($"{urlBase}/{id}");
            return !respuesta.Error;
        }

        //Task<List<GetCategoriasDTO>> ICategoriaServicios1.GetCategoriasDTO()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

