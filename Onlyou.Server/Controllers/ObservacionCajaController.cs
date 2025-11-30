using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.ObservacionCaja;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Cajas/{cajaId:int}/Observaciones")]
    public class ObservacionCajaController : ControllerBase
    {
        private readonly IRepositorioObservacionCaja repositorioObservacionCaja;
        private readonly IRepositorioCaja repositorioCaja;
        private readonly IMapper mapper;

        public ObservacionCajaController(IRepositorioObservacionCaja repositorioObservacionCaja, IRepositorioCaja repositorioCaja, IMapper mapper)
        {
            this.repositorioObservacionCaja = repositorioObservacionCaja;
            this.repositorioCaja = repositorioCaja;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<GetObservacionCajaDTO>>> GetObservaciones(int cajaId)
        {
            var Obs = await repositorioObservacionCaja.ListarObservacionesAsync(cajaId);
            var ObsDto = mapper.Map<List<GetObservacionCajaDTO>>(Obs);
            return Ok(ObsDto);
        }


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetObservacionCajaDTO>>> GetArchivados()
        {
            try
            {
                var obs = await repositorioObservacionCaja.SelectArchivados();
                if (obs == null || !obs.Any())
                    return NotFound("No hay Observaciones archivados.");

                var obsArchivadasDTO = mapper.Map<List<GetObservacionCajaDTO>>(obs);
                return Ok(obsArchivadasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<ActionResult<GetObservacionCajaDTO>> CrearObservacion(int cajaId, PostObservacionCajaDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar existencia de Caja
            var cajaExiste = await repositorioCaja.Existe(cajaId);
            if (!cajaExiste)
                return NotFound($"La caja {cajaId} no existe.");

            var obs = await repositorioObservacionCaja.AgregarObservacionAsync(cajaId, dto.Texto);

            var obsDto = mapper.Map<GetObservacionCajaDTO>(obs);

            return CreatedAtAction(nameof(GetObservaciones), new { cajaId }, obsDto);
        }

        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioObservacionCaja.UpdateEstado(id);
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
