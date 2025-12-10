using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.ObservacionPedido;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Pedidos/{pedidoId:int}/Observaciones")]
    public class ObservacionPedidoController : ControllerBase
    {
        private readonly IRepositorioObservacionPedido repositorioObservacionPedido;
        private readonly IRepositorioPedido repositorioPedido;
        private readonly IMapper mapper;

        public ObservacionPedidoController(IRepositorioObservacionPedido repositorioObservacionPedido, IRepositorioPedido repositorioPedido, IMapper mapper)
        {
            this.repositorioObservacionPedido = repositorioObservacionPedido;
            this.repositorioPedido = repositorioPedido;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetObservacionPedidoDTO>>> GetObservaciones(int pedidoId)
        {
            var Obs = await repositorioObservacionPedido.ListarObservacionesAsync(pedidoId);
            var ObsDto = mapper.Map<List<GetObservacionPedidoDTO>>(Obs);
            return Ok(ObsDto);
        }


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetObservacionPedidoDTO>>> GetArchivados()
        {
            try
            {
                var obs = await repositorioObservacionPedido.SelectArchivados();
                if (obs == null || !obs.Any())
                    return NotFound("No hay Observaciones archivados.");

                var obsArchivadasDTO = mapper.Map<List<GetObservacionPedidoDTO>>(obs);
                return Ok(obsArchivadasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<ActionResult<GetObservacionPedidoDTO>> CrearObservacion(int pedidoId, PostObservacionPedidoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar existencia de Caja
            var cajaExiste = await repositorioPedido.Existe(pedidoId);
            if (!cajaExiste)
                return NotFound($"La caja {pedidoId} no existe.");

            var obs = await repositorioObservacionPedido.AgregarObservacionAsync(pedidoId, dto.Texto);

            var obsDto = mapper.Map<GetObservacionPedidoDTO>(obs);

            return CreatedAtAction(nameof(GetObservaciones), new { pedidoId }, obsDto);
        }

        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioObservacionPedido.UpdateEstado(id);
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
