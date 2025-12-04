using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Categorias;
using Onlyou.Shared.DTOS.Producto;
using Onlyou.Shared.DTOS.TipoMovimento;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("TipoMovimientos")]
    public class TipoMovimientoController : ControllerBase
    {
        private readonly IRepositorioTipoMovimiento repositorioTipoMovimiento;
        private readonly IMapper mapper;

        public TipoMovimientoController(IRepositorioTipoMovimiento repositorioTipoMovimiento, IMapper mapper)
        {
            this.repositorioTipoMovimiento = repositorioTipoMovimiento;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<GetTipoMovimeintoDTO>>> GetListaTipoMovimientos()
        {
            try
            {
                var TipoMovimientos = await repositorioTipoMovimiento.Select();
                if (TipoMovimientos == null || !TipoMovimientos.Any())
                {
                    return Ok(new List<GetTipoMovimeintoDTO>()); ;
                }

                var TipoMovimientoDTO = mapper.Map<List<GetTipoMovimeintoDTO>>(TipoMovimientos);

                return Ok(TipoMovimientoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetListaTipoMovimientos: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetCategoriasDTO>> GetById(int id)
        {
            try
            {
                var TipoMovimientos = await repositorioTipoMovimiento.SelectById(id);
                if (TipoMovimientos == null)
                    return NotFound($"No se encontró un Tipo de Movimientos con el ID: {id}");

                var TipoMovimientoDTO = mapper.Map<GetTipoMovimeintoDTO>(TipoMovimientos);
                return Ok(TipoMovimientoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetTipoMovimeintoDTO>>> GetArchivados()
        {
            try
            {
                var TipoMovimientos = await repositorioTipoMovimiento.SelectArchivados();
                if (TipoMovimientos == null || !TipoMovimientos.Any())
                    return NotFound("No hay Tipo de Movimiento archivados.");

                var TipoMovimientoArchivadosDTO = mapper.Map<List<GetTipoMovimeintoDTO>>(TipoMovimientos);
                return Ok(TipoMovimientoArchivadosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPost("Filtrar")]
        public async Task<ActionResult<List<GetTipoMovimeintoDTO>>> Filtrar([FromBody] Dictionary<string, object?> filtros)
        {
            var TipoMovimientos = await repositorioTipoMovimiento.FiltrarConRelacionesAsync(filtros);
            var dto = mapper.Map<List<GetTipoMovimeintoDTO>>(TipoMovimientos);
            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult<GetTipoMovimeintoDTO>> CrearEntidad(PostTipoMovimientoDTO postTipoMovimientoDTO)
        {
            try
            {
                if (postTipoMovimientoDTO == null)
                {
                    return BadRequest("Favor de verificar, valor ingresado nulo.");
                }

                var TipoMovimiento = mapper.Map<TipoMovimiento>(postTipoMovimientoDTO);
                var TipoMovimientoDTO = await repositorioTipoMovimiento.InsertDevuelveDTO<GetTipoMovimeintoDTO>(TipoMovimiento);

                return Ok(TipoMovimientoDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPut("ModificarTMovimientoId/{id}")]
        public async Task<ActionResult<GetTipoMovimeintoDTO>> ModificarTMovimiento(int id, PutTipoMovimiento putTipoMovimiento)
        {
            try
            {
                var TipoMovimiento = await repositorioTipoMovimiento.SelectById(id);

                if (TipoMovimiento == null)
                {
                    return NotFound($"No se encontro un Tipo de Movimiento con el Id {id} .");
                }

                mapper.Map(putTipoMovimiento, TipoMovimiento);

                await repositorioTipoMovimiento.UpdateEntidad(id, TipoMovimiento);

                var TipoMovimientoDTO = mapper.Map<GetTipoMovimeintoDTO>(TipoMovimiento);

                return Ok(TipoMovimientoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el metodo ModificarTMovimiento: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }


        }

        [HttpPut("Archivar/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioTipoMovimiento.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }



        [HttpDelete("EliminarTipoMovimiento/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                await repositorioTipoMovimiento.EliminarTMovimientoAsync(id);
                return Ok("Tipo de Movimiento Eliminada Correctamente");
            }
            catch (InvalidOperationException ex)
            {
                //si hay productos asociados se lanza esta ex
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }


    }

}
