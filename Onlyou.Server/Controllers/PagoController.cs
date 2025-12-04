using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.Pago;
using Onlyou.Shared.Enums;

namespace Onlyou.Server.Controllers
{
    [Route("/Pagos")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        private readonly IRepositorioPago repositorioPago;
        private readonly IRepositorioTipoPago repositorioTipoPago;
        private readonly IRepositorioMovimiento repositorioMovimiento;
        private readonly IPagoService pagoService;
        private readonly IMapper mapper;

        public PagoController(
            IRepositorioPago repositorioPago,
            IRepositorioTipoPago repositorioTipoPago,
            IRepositorioMovimiento repositorioMovimiento,
            IPagoService pagoService,
            IMapper mapper)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioTipoPago = repositorioTipoPago;
            this.repositorioMovimiento = repositorioMovimiento;
            this.pagoService = pagoService;
            this.mapper = mapper;
        }

        // ==========================================
        // GET ALL
        // ==========================================
        [HttpGet]
        public async Task<ActionResult<List<GetPagoDTO>>> GetAll()
        {
            try
            {
                var pagos = await repositorioPago.SelectConRelaciones();
                if (pagos == null)
                    return NotFound("No se encontraron Pagos que mostrar");

                var pagosDTO = mapper.Map<List<GetPagoDTO>>(pagos);
                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAll: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // ==========================================
        // GET BY ID
        // ==========================================
        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetPagoDTO>> GetById(int id)
        {
            if (id == 0)
                return BadRequest($"El id no puede ser '0'");

            try
            {
                var pago = await repositorioPago.SelectConRelacionesXId(id);
                if (pago == null)
                    return BadRequest($"No se encontró un Pago con ID '{id}'");

                var pagoDTO = mapper.Map<GetPagoDTO>(pago);
                return Ok(pagoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // ==========================================
        // GET ARCHIVADOS
        // ==========================================
        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetPagoDTO>>> GetArchivados()
        {
            try
            {
                var pagos = await repositorioPago.SelectArchivadosConRelaciones();
                if (pagos == null || !pagos.Any())
                    return NotFound("No hay pagos archivados.");

                var pagosDTO = mapper.Map<List<GetPagoDTO>>(pagos);
                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // ==========================================
        // FILTRAR
        // ==========================================
        [HttpPost("Filtrar")]
        public async Task<ActionResult<List<GetPagoDTO>>> Filtrar([FromBody] Dictionary<string, object?> filtros)
        {
            var pagos = await repositorioPago.FiltrarConRelacionesAsync(filtros);
            return Ok(mapper.Map<List<GetPagoDTO>>(pagos));
        }

        // ==========================================
        // POST — REGISTRA PAGO (AHORA POR SERVICE)
        // ==========================================
        [HttpPost]
        public async Task<ActionResult<GetPagoDTO>> Post(PostPagoDTO postPagoDTO)
        {
            try
            {
                if (postPagoDTO == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                // ---- Validaciones actuales ----
                var mov = await repositorioMovimiento.SelectMovimientoPorIdAsync(postPagoDTO.MovimientoId);
                if (mov == null)
                    return NotFound($"No existe un movimiento con ID {postPagoDTO.MovimientoId}.");

                var tipoPago = await repositorioTipoPago.SelectTipoPagosxId(postPagoDTO.TipoPagoId);
                if (tipoPago == null)
                    return NotFound($"No existe un tipo de pago con ID {postPagoDTO.TipoPagoId}.");

                if (!Enum.IsDefined(typeof(SituacionPagoDto), postPagoDTO.Situacion))
                    return BadRequest("La situación del pago no es válida.");

                decimal totalPagado = await repositorioPago.SelectTotalPagosPorMovimientoAsync(postPagoDTO.MovimientoId);
                decimal saldo = mov.Monto - totalPagado;

                if (postPagoDTO.Monto > saldo && !postPagoDTO.EsPagoCliente)
                    return BadRequest($"El monto ({postPagoDTO.Monto}) excede el saldo ({saldo}).");

                if (!string.IsNullOrWhiteSpace(postPagoDTO.Descripcion))
                    postPagoDTO.Descripcion = postPagoDTO.Descripcion.Trim();

                // ---- Mapear ----
                var pago = mapper.Map<Pago>(postPagoDTO);
                pago.Situacion = (Situacion)postPagoDTO.Situacion;

                // ⚡ NUEVO → Guardar usando SERVICE (impacta caja y recalcula movimiento)
                var pagoRegistrado = await pagoService.RegistrarPagoAsync(pago);

                var dto = mapper.Map<GetPagoDTO>(pagoRegistrado);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en POST Pago: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // ==========================================
        // PUT — CAMBIAR SITUACIÓN (AHORA POR SERVICE)
        // ==========================================
        [HttpPut("{id:int}/situacion")]
        public async Task<ActionResult<GetPagoDTO>> CambiarSituacion(int id, [FromBody] PutPagoSituacionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // ⚡ NUEVO → Cambiar situación con lógica contable
                var pago = await pagoService.CambiarSituacionAsync(
                    id,
                    (Situacion)dto.NuevaSituacion,
                    dto.Observacion
                );

                return Ok(mapper.Map<GetPagoDTO>(pago));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CambiarSituacion Pago: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // ==========================================
        // ARCHIVAR / BAJA LÓGICA (sin cambios)
        // ==========================================
        [HttpPut("UpdateEstado/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioPago.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Archivar Pago: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
    }
}
