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
        private readonly IMapper mapper;

        public ObservacionCajaController(IRepositorioObservacionCaja repositorioObservacionCaja, IMapper mapper)
        {
            this.repositorioObservacionCaja = repositorioObservacionCaja;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<GetObservacionCajaDTO>> GetObservaciones(int cajaId)
        {
            var Obs = await repositorioObservacionCaja.ListarObservacionesAsync(cajaId);
            var ObsDto = mapper.Map<IEnumerable<GetObservacionCajaDTO>>(Obs);
            return Ok(ObsDto);
        }

        [HttpPost]
        public async Task<ActionResult<PostObservacionCajaDTO>> CreaarObservacion(int cajaId, PostObservacionCajaDTO postObservacionCajaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);                
            }

            var Obs = await repositorioObservacionCaja.AgregarObservacionAsync(cajaId, postObservacionCajaDTO.Texto);

            var ObsDto = mapper.Map<PostObservacionCajaDTO>(Obs);
            return Ok(ObsDto);

        }

        [HttpPut("UpdateEstado/{id}")]
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
