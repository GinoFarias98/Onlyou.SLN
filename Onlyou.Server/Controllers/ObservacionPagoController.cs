using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.ObservacionCaja;
using Onlyou.Shared.DTOS.ObservacionPago;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Pagos/{pagoId:int}/Observaciones")]
    public class ObservacionPagoController : ControllerBase
    {
        private readonly IRepositorioObservacionPago repositorioObservacionPago;
        private readonly IRepositorioPago repositorioPago;
        private readonly IMapper mapper;

        public ObservacionPagoController(IRepositorioObservacionPago repositorioObservacionPago,IRepositorioPago repositorioPago, IMapper mapper)
        {
            this.repositorioObservacionPago = repositorioObservacionPago;
            this.repositorioPago = repositorioPago;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetObservacionPagoDTO>>> GetObservaciones(int pagoId)
        {
            var Obs = await repositorioObservacionPago.ListarObservacionesAsync(pagoId);
            var ObsDto = mapper.Map<List<GetObservacionPagoDTO>>(Obs);
            return Ok(ObsDto);
        }


        [HttpPost]
        public async Task<ActionResult<GetObservacionPagoDTO>> CrearObservacion(int pagoId, PostObservacionPagoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar existencia de Caja
            var pagoExiste = await repositorioPago.Existe(pagoId);
            if (!pagoExiste)
                return NotFound($"La caja {pagoId} no existe.");

            var obs = await repositorioObservacionPago.AgregarObservacionAsync(pagoId, dto.Texto);

            var obsDto = mapper.Map<GetObservacionPagoDTO>(obs);

            return CreatedAtAction(nameof(GetObservaciones), new { pagoId }, obsDto);
        }

        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioObservacionPago.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }
    }
}
