using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.Caja;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Cajas")]
    public class CajaController : ControllerBase
    {
        private readonly IRepositorioCaja repoCaja;
        private readonly ICajaService cajaService;
        private readonly IMapper mapper;

        public CajaController(
            IRepositorioCaja repoCaja,
            ICajaService cajaService,
            IMapper mapper)
        {
            this.repoCaja = repoCaja;
            this.cajaService = cajaService;
            this.mapper = mapper;
        }

        // ======================================================
        // GET - Lecturas
        // ======================================================

        [HttpGet("abierta")]
        public async Task<ActionResult<GetCajaDTO>> GetCajaAbierta()
        {
            try
            {
                var caja = await repoCaja.SelectCajaAbiertaAsync();
                if (caja == null)
                    return NotFound("No hay ninguna Caja abierta actualmente");

                return Ok(mapper.Map<GetCajaDTO>(caja));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("cerradas")]
        public async Task<ActionResult<List<GetCajaDTO>>> GetCajasCerradas()
        {
            try
            {
                var cajas = await repoCaja.ListarCajasCerradasAsync();
                return Ok(mapper.Map<List<GetCajaDTO>>(cajas));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("por-rango")]
        public async Task<ActionResult<List<GetCajaDTO>>> GetPorRango(DateTime inicio, DateTime fin)
        {
            try
            {
                if (inicio > fin)
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha final.");

                var cajas = await repoCaja.SelectCajasPorRangoFechasAsync(inicio, fin);
                return Ok(mapper.Map<List<GetCajaDTO>>(cajas));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        // ======================================================
        // POST - Abrir Nueva Caja
        // ======================================================

        [HttpPost("abrir")]
        public async Task<ActionResult<GetCajaDTO>> AbrirCaja([FromBody] PostCajaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var caja = await cajaService.AbrirCajaAsync(dto.SaldoInicial);

                return Ok(mapper.Map<GetCajaDTO>(caja));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        // ======================================================
        // PUT - Cambiar Estado / Cerrar caja
        // ======================================================

        [HttpPut("{id:int}/cambiar-estado")]
        public async Task<ActionResult<GetCajaDTO>> CambiarEstado(int id, [FromBody] PutEstadoCajaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var nuevoEstado = mapper.Map<Caja.EstadoCaja>(dto.EstadoCaja);

                // Si quiere cerrar la caja → usar service (tiene lógica)
                if (nuevoEstado == Caja.EstadoCaja.Cerrada)
                {
                    await cajaService.CerrarCajaAsync(id, dto.Observacion);
                }
                else
                {
                    // Para abrir o anular → repositorio directo
                    await repoCaja.CambiarEstadoCajaAsync(id, nuevoEstado, dto.Observacion);
                }

                var cajaActualizada = await repoCaja.SelectById(id);
                return Ok(mapper.Map<GetCajaDTO>(cajaActualizada));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
