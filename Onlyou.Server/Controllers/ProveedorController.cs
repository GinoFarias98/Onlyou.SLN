using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Color;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.Proveedor;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/Proveedores")]
    public class ProveedorController : ControllerBase
    {
        private readonly IRepositorioProveedor repoProveedor;
        private readonly IMapper mapper;

        public ProveedorController(IRepositorioProveedor repoProveedor, IMapper mapper)
        {
            this.repoProveedor = repoProveedor;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetProveedorDTO>>> GetProveedores()
        {
            try
            {
                var provs = await repoProveedor.Select();

                if (provs == null)
                {
                    return BadRequest("No se encontraron Colores que mostrar");
                }

                var provsDTO = mapper.Map<List<GetProveedorDTO>>(provs);

                return Ok(provsDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GET: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetProveedorDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest($"El id no puede ser '0', favor verificar ingreso de datos");

            }

            try
            {
                var prov = await repoProveedor.SelectById(id);
                if (prov == null)
                {

                    return BadRequest($"No se encontro una Marca con el ID '{id}' que mostrar");

                }

                var provDTO = mapper.Map<GetMarcaDTO>(prov);
                return Ok(provDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetByCodigo: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<GetProveedorDTO>> Post(PostProveedorDTO postMarcaDTO)
        {
            try
            {
                if (postMarcaDTO == null)
                {
                    return BadRequest($"Favor de verificar, valor ingresado nulo");
                }

                var prov = mapper.Map<Proveedor>(postMarcaDTO);
                var dto = await repoProveedor.InsertDevuelveDTO<GetProveedorDTO>(prov);

                return Ok(dto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPut("ModificarProveedorId/{id}")]
        public async Task<ActionResult<GetMarcaDTO>> Put(int id, [FromBody] PutProveedorDTO putProveedorDTO)
        {
            try
            {
                var prov = await repoProveedor.SelectById(id);
                if (prov == null)
                {
                    return BadRequest($"No se encontró un Color con Id {id}.");
                }
                // Mapear los cambios del DTO al color
                mapper.Map(putProveedorDTO, prov);

                // Actualizar en la base de datos
                await repoProveedor.UpdateEntidad(id, prov);

                // Devolver un DTO de lectura para mostrar en el frontend
                var provDto = mapper.Map<GetProveedorDTO>(prov);

                return Ok(provDto); // ✅ Devuelve el proveedor actualizado
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el metodo Put: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPut("Archivar/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repoProveedor.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }


        [HttpDelete("EliminarCodigo/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var entidad = await repoProveedor.SelectById(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Color con Id {id}. Favor verificar");
                }

                var eliminado = await repoProveedor.Delete(entidad.Id);

                if (eliminado)
                {
                    return Ok($"El Proveedor con Id {id} fue eliminado");
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
