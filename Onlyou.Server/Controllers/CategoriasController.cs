using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Categorias;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IRepositorio<Categoria> repositorio;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheKey = "Categorias";

        public CategoriasController(
            IRepositorio<Categoria> repositorio,
            IMapper mapper,
            IOutputCacheStore outputCacheStore)
        {
            this.repositorio = repositorio;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }

        // GET: api/categorias
        [HttpGet]
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<List<GetCategoriasDTO>>> GetCategorias()
        {
            try
            {
                var categorias = await repositorio.Select();
                if (categorias == null || !categorias.Any())
                {
                    return Ok(new List<GetCategoriasDTO>()); // Lista vacía coherente
                }

                var categoriasDTO = mapper.Map<List<GetCategoriasDTO>>(categorias);
                return Ok(categoriasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetCategorias: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/categorias/5
        [HttpGet("{id:int}")]
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult<GetCategoriasDTO>> GetById(int id)
        {
            try
            {
                var categoria = await repositorio.SelectById(id);
                if (categoria == null)
                    return NotFound($"No se encontró una Categoría con el ID: {id}");

                var dto = mapper.Map<GetCategoriasDTO>(categoria);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCategoriasDTO dto)
        {
            try
            {
                var entidad = mapper.Map<Categoria>(dto);

                // 🔥 Si Codigo viene null o vacío, lo asignamos
                if (string.IsNullOrWhiteSpace(entidad.Codigo))
                {
                    entidad.Codigo = Guid.NewGuid().ToString().Substring(0, 8); // o ""
                }

                var id = await repositorio.Insert(entidad);
                await outputCacheStore.EvictByTagAsync(cacheKey, default);

                return Ok(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Post: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner Exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Ocurrió un error interno: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // PUT: api/categorias/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, EditarCategoriasDTO dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("IDs no coinciden");

                var existente = await repositorio.SelectById(id);
                if (existente == null)
                    return NotFound("No existe la categoría");

                if (string.IsNullOrWhiteSpace(dto.Nombre))
                    return BadRequest("El nombre es obligatorio");

                mapper.Map(dto, existente);
                var ok = await repositorio.UpdateEntidad(id, existente);

                await outputCacheStore.EvictByTagAsync(cacheKey, default);
                return ok ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // DELETE: api/categorias/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var entidad = await repositorio.SelectById(id);
                if (entidad == null)
                    return NotFound($"No existe la categoría con Id {id}");

                var ok = await repositorio.Delete(entidad.Id);
                if (ok)
                {
                    await outputCacheStore.EvictByTagAsync(cacheKey, default);
                    return Ok($"Categoría {id} eliminada");
                }

                return BadRequest("No se pudo eliminar");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
    }
}
