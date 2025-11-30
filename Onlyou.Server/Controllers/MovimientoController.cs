using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Movimiento;
using Onlyou.Shared.DTOS.Producto;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("/Movimientos")]
    public class MovimientoController : ControllerBase
    {
        private readonly IRepositorioMovimiento repositorioMovimiento;
        private readonly IMapper mapper;

        public MovimientoController(IRepositorioMovimiento repositorioMovimiento, IMapper mapper)
        {
            this.repositorioMovimiento = repositorioMovimiento;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetMovimientoDTO>>> GetAll()
        {
            try
            {
                var lista = await repositorioMovimiento.Select();
                var dto = mapper.Map<List<GetMovimientoDTO>>(lista);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAll: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetMovimientoDTO>> GetById(int id)
        {
            try
            {
                var mov = await repositorioMovimiento.SelectMovimientoPorIdAsync(id);
                var dto = mapper.Map<GetMovimientoDTO>(mov);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetById: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("por-caja/{cajaId:int}")]
        public async Task<ActionResult<List<GetMovimientoDTO>>> GetPorCaja(int cajaId)
        {
            try
            {
                var lista = await repositorioMovimiento.SelectMovimientosPorCajaAsync(cajaId);
                var dto = mapper.Map<List<GetMovimientoDTO>>(lista);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetPorCaja: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("por-fecha")]
        public async Task<ActionResult<List<GetMovimientoDTO>>> GetPorFecha([FromQuery] DateTime fecha)
        {
            try
            {
                var lista = await repositorioMovimiento.SelectMovimientosPorFechaAsync(fecha);
                return Ok(mapper.Map<List<GetMovimientoDTO>>(lista));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetPorFecha: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetMovimientoDTO>>> GetArchivados()
        {
            try
            {
                var movimientos = await repositorioMovimiento.SelectArchivadosConRelaciones();
                if (movimientos == null || !movimientos.Any())
                    return NotFound("No hay movimientos archivados.");

                var movimientosArchivadosDTO = mapper.Map<List<GetMovimientoDTO>>(movimientos);
                return Ok(movimientosArchivadosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPost("Filtrar")]
        public async Task<ActionResult<List<GetMovimientoDTO>>> Filtrar([FromBody] Dictionary<string, object?> filtros)
        {
            var movimientos = await repositorioMovimiento.FiltrarConRelacionesAsync(filtros);
            var dto = mapper.Map<List<GetMovimientoDTO>>(movimientos);
            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult<GetMovimientoDTO>> Post([FromBody] PostMovimientoDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entidad = mapper.Map<Movimiento>(dto);
                var id = await repositorioMovimiento.Insert(entidad);

                var creado = await repositorioMovimiento.SelectMovimientoPorIdAsync(id);
                return Ok(mapper.Map<GetMovimientoDTO>(creado));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Post: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GetMovimientoDTO>> Put(int id, [FromBody] PutMovimientoDTO dto)
        {
            try
            {
                var mov = await repositorioMovimiento.SelectMovimientoPorIdAsync(id);
                if (mov == null)
                    return NotFound("Movimiento no encontrado.");

                mapper.Map(dto, mov);

                await repositorioMovimiento.UpdateEntidad(id, mov);

                return Ok(mapper.Map<GetMovimientoDTO>(mov));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{id:int}/cambiar-estado")]
        public async Task<ActionResult<GetMovimientoDTO>> CambiarEstado(int id, [FromBody] PutEstadoMovimientoDTO dto)
        {
            try
            {
                var nuevoEstado = mapper.Map<EstadoMovimiento>(dto.EstadoMovimiento);
                var mov = await repositorioMovimiento.CambiarEstadoMovimientoAsync(id, nuevoEstado, dto.Descripcion);

                return Ok(mapper.Map<GetMovimientoDTO>(mov));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CambiarEstado: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var ok = await repositorioMovimiento.Delete(id);
                if (!ok) return NotFound("Movimiento no encontrado.");

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Delete: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

    }
}
