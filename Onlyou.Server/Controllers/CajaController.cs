using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Caja;
using Onlyou.Shared.DTOS.Producto;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Cajas")]
    public class CajaController : ControllerBase
    {
        private readonly IRepositorioCaja repoCaja;
        private readonly IMapper mapper;

        public CajaController(IRepositorioCaja repoCaja, IMapper mapper)
        {
            this.repoCaja = repoCaja;
            this.mapper = mapper;
        }

        #region GET - OBTENER DATOS

        [HttpGet("abierta")]
        public async Task<ActionResult<GetCajaDTO>> GetCajaAbierta()
        {
            try
            {
                var caja = await repoCaja.SelectCajaAbiertaAsync();
                if (caja == null)
                {
                    return NotFound("No hay ninguna Caja abierta actualmente");
                }

                var DtoCaja = mapper.Map<GetCajaDTO>(caja);

                return Ok(DtoCaja);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetCajaAbierta: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpGet("cerradas")]
        public async Task<ActionResult<List<List<GetCajaDTO>>>> GetCajasCerradas()
        {
            try
            {
                var cajas = await repoCaja.ListarCajasCerradasAsync();
                if (cajas == null)
                {
                    return NotFound("No se encontraron cajas cerradas que mostrar");
                }

                var DtoCajas = mapper.Map<List<GetCajaDTO>>(cajas);

                return Ok(DtoCajas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetCajasCerradas: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

        [HttpGet("por-rango")]
        public async Task<ActionResult<List<GetCajaDTO>>> GetCajasPorRango([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {

            try
            {
                if (inicio == default || fin == default)
                    return BadRequest("Debe proporcionar fechas válidas.");

                if (inicio.Date > fin.Date)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");


                var cajas = await repoCaja.SelectCajasPorRangoFechasAsync(inicio, fin);
                var DtoCajas = mapper.Map<List<GetCajaDTO>>(cajas);
                return Ok(DtoCajas);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetCajasPorRango: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }


        #endregion



        #region Abrir nueva Caja
        [HttpPost("abrir")]
        public async Task<ActionResult<GetCajaDTO>> AbrirCaja([FromBody] PostCajaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Abrir caja usando el repositorio
                var nuevaCaja = await repoCaja.AbrirNuevaCajaAsync(dto.SaldoInicial);

                // Mapear la entidad a un DTO de salida
                var cajaDTO = mapper.Map<GetCajaDTO>(nuevaCaja);

                return Ok(cajaDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // caja ya abierta, etc.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método AbrirCaja: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
        #endregion

        #region Metodos PUT

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GetCajaDTO>> ActualizarCaja(int id, [FromBody] PutCajaDTO putCajaDTO)
        {
            try
            {
                var caja = await repoCaja.SelectById(id);
                if (caja == null)
                {
                    return NotFound("Caja no encontrada.");
                }

                mapper.Map(putCajaDTO, caja);

                await repoCaja.UpdateEntidad(id, caja);

                var DtoCaja = mapper.Map<GetCajaDTO>(caja);
                return Ok(DtoCaja);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método ActualizarCaja: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }

        [HttpPut("{id:int}/cambiar-estado")]
        public async Task<ActionResult<GetCajaDTO>> CambiarEstado(int id, [FromBody] PutEstadoCajaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var nuevoEstado = mapper.Map<Caja.EstadoCaja>(dto.estadoCaja);
                var caja = await repoCaja.CambiarEstadoCajaAsync(id, nuevoEstado, dto.Observacion);

                var dtoCaja = mapper.Map<GetCajaDTO>(caja);
                return Ok(dtoCaja);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método CambiarEstado: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }
        #endregion




    }
}
