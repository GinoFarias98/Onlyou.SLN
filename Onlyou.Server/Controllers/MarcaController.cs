using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Marca;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/Marca")]
    public class MarcaController : ControllerBase
    {
        private readonly IRepositorioMarca repoMarca;
        private readonly IMapper mapper;

        public MarcaController(IRepositorioMarca repoMarca, IMapper mapper)
        {
            this.repoMarca = repoMarca;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetMarcaDTO>>> GetMarcas()
        {
            try
            {
                var marcas = await repoMarca.Select();

                if (marcas == null)
                {
                    return BadRequest("No se encontraron MArcas que mostrar");
                }

                var marcasDTO = mapper.Map<List<GetMarcaDTO>>(marcas);

                return Ok(marcasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GET: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpGet("Codigo/{codigo}")]
        public async Task<ActionResult<GetMarcaDTO>> GetByCodigo(string codigo)
        {
            try
            {
                var marca = await repoMarca.SelectByCod(codigo);

                if (marca == null)
                {
                    return BadRequest($"No se encontro una Marca con el CODIGO '{codigo}' que mostrar");
                }
                var marcaDto = mapper.Map<GetMarcaDTO>(marca);
                return Ok(marcaDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetByCodigo: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetMarcaDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest($"El id no puede ser '0', favor verificar ingreso de datos");

            }

            try
            {
                var marca = await repoMarca.SelectById(id);
                if (marca == null)
                {

                    return BadRequest($"No se encontro una Marca con el ID '{id}' que mostrar");

                }

                var marcaDTO = mapper.Map<GetMarcaDTO>(marca);
                return Ok(marcaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetByCodigo: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<GetMarcaDTO>> Post(PostMarcaDTO postMarcaDTO)
        {
            try
            {
                if (postMarcaDTO == null)
                {
                    return BadRequest($"Favor de verificar, valor ingresado nulo");
                }

                var marca = mapper.Map<Marca>(postMarcaDTO);
                var dto = await repoMarca.InsertDevuelveDTO<GetMarcaDTO>(marca);

                return Ok(dto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPut("ModificarMarcaId/{id}")]
        public async Task<ActionResult<GetMarcaDTO>> Put(int id, [FromBody] PutMarcaDTO putMarcaDTO)
        {
            try
            {
                var marca = await repoMarca.SelectById(id);

                if (marca == null)
                {
                    return BadRequest($"No se encontró un Color con Id {id}.");
                }
                // Mapear los cambios del DTO al color
                mapper.Map(putMarcaDTO, marca);

                // Actualizar en la base de datos
                await repoMarca.UpdateEntidad(id, marca);

                // Devolver un DTO de lectura para mostrar en el frontend
                var marcaDTO = mapper.Map<GetMarcaDTO>(marca);

                return Ok(marcaDTO); // ✅ Devuelve el color actualizado
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el metodo Put: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpDelete("EliminarCodigo/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var entidad = await repoMarca.SelectById(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Color con Id {id}. Favor verificar");
                }

                var eliminado = await repoMarca.Delete(entidad.Id);

                if (eliminado)
                {
                    return Ok($"La Marca con Id {id} fue eliminado");
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
