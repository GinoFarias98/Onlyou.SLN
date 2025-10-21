using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.Categorias;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IRepositorio<Categoria> repositorio;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheKey = "Categorias";
        private readonly string contenedor = "categorias";


        public CategoriasController(IRepositorio<Categoria> repositorio, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore)
        {
            this.repositorio = repositorio;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.outputCacheStore = outputCacheStore;
        }


        /// <summary>
        /// Actualiza la imagen de una categoría si se envía una nueva.
        /// Devuelve la ruta final de la imagen en el almacenamiento.
        /// </summary>
        /// <returns></returns>
        private async Task<string> ActualizarImagenCategoria(
            string imagenBase64,
            string imagenExtension,
            string imagenActual,
            string contenedor,
            [FromServices] IImagenValidator validator)
        {
            if (string.IsNullOrWhiteSpace(imagenBase64))
            {
                return imagenActual; // no hay cambio, devolvemos imagen existente
            }

            // validacion
            if (!validator.ValidarBase64Extencion(imagenBase64, imagenExtension))
            {
                throw new InvalidOperationException("El contenido de la imagen no coincide con la extencion indicada");
            }

            var archivoBytes = Convert.FromBase64String(imagenBase64);

            // si la imagen ya existe borramos
            if (!string.IsNullOrEmpty(imagenActual))
            {
                await almacenadorArchivos.EliminarArchivo(imagenActual, contenedor);
            }

            //guardamos img nueva

            var rutaimg = await almacenadorArchivos.GuardarArchivo(archivoBytes, imagenExtension, contenedor);
            return rutaimg;
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


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetCategoriasDTO>>> GetArchivados()
        {
            try
            {
                var categorias = await repositorio.SelectArchivados();
                if (categorias == null || !categorias.Any())
                    return NotFound("No hay productos archivados.");

                var categoriasArchivadasDTO = mapper.Map<List<GetCategoriasDTO>>(categorias);
                return Ok(categoriasArchivadasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCategoriasDTO dto, [FromServices] IImagenValidator validator)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Favor de verificar, valor ingresado nulo.");
                }

                if (!string.IsNullOrWhiteSpace(dto.Imagen) && !string.IsNullOrWhiteSpace(dto.ImagenExtension))
                {
                    if (!validator.ValidarBase64Extencion(dto.Imagen, dto.ImagenExtension))
                    {
                        return BadRequest("El contenido de la imagen no coincide con la extensión indicada.");

                    }

                    var imgCategoria = Convert.FromBase64String(dto.Imagen);

                    dto.Imagen = await almacenadorArchivos.GuardarArchivo(
                        imgCategoria,
                        dto.ImagenExtension,
                        contenedor
                    );
                }

                // mapeo

                var categoria = mapper.Map<Categoria>(dto);
                var categoriadto = await repositorio.InsertDevuelveDTO<GetCategoriasDTO>(categoria);

                await outputCacheStore.EvictByTagAsync(cacheKey, default);

                return Ok(categoriadto);

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
        public async Task<ActionResult> Put(int id, [FromBody] EditarCategoriasDTO editarCategoriasDTO, [FromServices] IImagenValidator validator)
        {
            try
            {
                if (editarCategoriasDTO == null)
                {
                    return BadRequest("Datos nulos, Favor verificar ingreso de datos.");
                }
                var categoria = await repositorio.SelectById(id);
                if (categoria == null)
                {
                    return NotFound($"No existe la Categoria buscada");
                }



                // 🖼️ 1️⃣ Manejo de imagen
                categoria.Imagen = await ActualizarImagenCategoria(
                    editarCategoriasDTO.Imagen,
                    editarCategoriasDTO.ImagenExtension,
                    categoria.Imagen,
                    contenedor,
                    validator);

                // 🔄 2️⃣ Aplicamos AutoMapper a los campos simples
                var catModificada = mapper.Map(editarCategoriasDTO, categoria);

                await repositorio.UpdateEntidad(catModificada.Id, catModificada);
                var categoriaActualizada = await repositorio.SelectById(catModificada.Id);
                var categoriaDTO = mapper.Map<GetCategoriasDTO>(categoriaActualizada);


                await outputCacheStore.EvictByTagAsync(cacheKey, default);
                return Ok(categoriaDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno al actualizar Categoria: {ex.Message}");
            }
        }

        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorio.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }

        // DELETE: api/categorias/5
        [HttpDelete("EliminarCategoria/{id:int}")]
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
