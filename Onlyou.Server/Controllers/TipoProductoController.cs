using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.TipoProducto;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/TipoProducto")]
    public class TipoProductoController : ControllerBase
    {
        private readonly IRepositorioTipoProducto repoTipoProducto;
        private readonly IMapper mapper;

        public TipoProductoController(IRepositorioTipoProducto repoTipoProducto,
                                      IMapper mapper)
        {
            this.repoTipoProducto = repoTipoProducto;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetTipoProductoDTO>>> GetTipoProductos()
        {
            try
            {
                var tipoProductos = await repoTipoProducto.Select();
                if (tipoProductos == null || !tipoProductos.Any())
                {
                    return Ok(new List<GetTipoProductoDTO>()); // Enviar lista vacía
                }

                var tipoProductosDTO = mapper.Map<List<GetTipoProductoDTO>>(tipoProductos);
                return Ok(tipoProductosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetTipoProductos: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetTipoProductoDTO>> GetById(int id)
        {
            try
            {
                var tipoProducto = await repoTipoProducto.SelectById(id);
                if (tipoProducto == null)
                {
                    return BadRequest($"No se encontro un Tipo de Producto con el ID: '{id}' que mostrar");
                }
                var tipoProducoDTO = mapper.Map<GetTipoProductoDTO>(tipoProducto);
                return Ok(tipoProducoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpGet("SimilName/{similName}")]
        public async Task<ActionResult<List<GetTipoProductoDTO>>> GetBySimilName(string similName)
        {

            try
            {
                var similTipoProductos = await repoTipoProducto.SelectBySimilName(similName);
                if (similTipoProductos == null)
                {
                    return BadRequest($"No se encontraron Tipo de Productos que mostrar con el NOMBRE: '{similName.ToUpper()}'");
                }
                var similTipoProductosDTO = mapper.Map<List<GetTipoProductoDTO>>(similTipoProductos);
                return Ok(similTipoProductosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetBySimilName: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<GetTipoProductoDTO>> Post(PostTipoProductoDTO postTipoProductoDTO)
        {
            try
            {
                if (postTipoProductoDTO == null)
                {
                    return BadRequest($"Favor de verificar, valor ingresado nulo");

                }

                var tipoProducto = mapper.Map<TipoProducto>(postTipoProductoDTO);
                var dto = await repoTipoProducto.InsertDevuelveDTO<GetTipoProductoDTO>(tipoProducto);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPut("ModificarTipoProducto/{id}")]
        public async Task<ActionResult<GetTipoProductoDTO>> Put(int id, [FromBody] PutTipoProductoDTO putTipoProductoDTO)
        {
            try
            {
                var tipoProducto = await repoTipoProducto.SelectById(id);
                if (tipoProducto == null)
                {
                    return BadRequest($"No se encontró un Tipo de Producto con Id {id}.");
                }
                mapper.Map(putTipoProductoDTO, tipoProducto);

                await repoTipoProducto.UpdateEntidad(id, tipoProducto);

                var tProductoDTO = mapper.Map<GetTipoProductoDTO>(tipoProducto);

                return Ok(tProductoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Tipo Producto: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPut("Archivar/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repoTipoProducto.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }


        [HttpDelete("EliminarTipoProducto/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var entidad = await repoTipoProducto.SelectById(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Tipo de Producto con Id {id}. Favor verificar");
                }

                var eliminado = await repoTipoProducto.Delete(entidad.Id);

                if (eliminado)
                {
                    return Ok($"El Tipo de Producto con Id {id} fue eliminado");
                }

                return BadRequest("No se pudo llevar a cabo la acción");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

    }
}
