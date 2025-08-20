using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Color;
using System.Drawing;
using Color = Onlyou.BD.Data.Entidades.Color;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/ManageColor")]
    public class ColorController : ControllerBase
    {
        private readonly IRepositorioColor repoColor;
        private readonly IMapper mapper;

        public ColorController(IRepositorioColor repoColor, IMapper mapper)
        {
            this.repoColor = repoColor;
            this.mapper = mapper;
        }

        // GET ---------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<List<GetColorDTO>>> GetColores()
        {
            try
            {
                var colores = await repoColor.Select();

                if (colores == null)
                {
                    return BadRequest("No se encontraron Colores que mostrar");
                }

                var coloresDTO = mapper.Map<List<GetColorDTO>>(colores);

                return Ok(coloresDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GET: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpGet("ObtenerHexa")]
        public async Task<ActionResult<string>> GetHexa(int idColor)
        {
            try
            {
                var hexa = await repoColor.ObtenerHexa(idColor);

                if (hexa == null)
                {
                    return BadRequest($"No se encontro un color con el ID {idColor} que mostrar");
                }

                return Ok(hexa);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetHexa: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Codigo/{codigo}")]
        public async Task<ActionResult<GetColorDTO>> GetByCodigo(string codigo)
        {
            try
            {
                var color = await repoColor.SelectByCod(codigo);

                if (color == null)
                {
                    return BadRequest($"No se encontro un color con el CODIGO '{codigo}' que mostrar");
                }
                var colorDto = mapper.Map<GetColorDTO>(color);
                return Ok(colorDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetByCodigo: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // POST ---------------------------------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<ActionResult<GetColorDTO>> Post(PostColorDTO postColorDTO)
        {
            try
            {
                if (postColorDTO == null)
                {
                    return BadRequest($"Favor de verificar, valor ingresado nulo");
                }

                var color = mapper.Map<Color>(postColorDTO);
                var dto = await repoColor.InsertDevuelveDTO(color);

                return Ok(dto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // PUT ---------------------------------------------------------------------------------------------------------------
        // PUT ---------------------------------------------------------------------------------------------------------------
        [HttpPut("ModificarColorId/{id}")]
        public async Task<ActionResult<GetColorDTO>> Put(int id, [FromBody] PutColorDTO putColorDTO)
        {
            try
            {
                var color = await repoColor.SelectById(id);

                if (color == null)
                    return BadRequest($"No se encontró un Color con Id {id}.");

                // Mapear los cambios del DTO al color
                mapper.Map(putColorDTO, color);

                // Actualizar en la base de datos
                await repoColor.UpdateEntidad(id, color);

                // Devolver un DTO de lectura para mostrar en el frontend
                var colorDTO = mapper.Map<GetColorDTO>(color);

                return Ok(colorDTO); // ✅ Devuelve el color actualizado
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PUT Color: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        // DELETE ---------------------------------------------------------------------------------------------------------------

        [HttpDelete("EliminarCodigo/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var entidad = await repoColor.SelectById(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Color con Id {id}. Favor verificar");
                }

                var eliminado = await repoColor.Delete(entidad.Id);

                if (eliminado)
                {
                    return Ok($"El Color con Id {id} fue eliminado");
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
