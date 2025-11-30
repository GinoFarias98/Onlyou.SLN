using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.Pago;
using Onlyou.Shared.DTOS.Producto;
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
        private readonly IMapper mapper;

        public PagoController(IRepositorioPago repositorioPago,IRepositorioTipoPago repositorioTipoPago, IRepositorioMovimiento repositorioMovimiento, IMapper mapper)
        {
            this.repositorioPago = repositorioPago;
            this.repositorioTipoPago = repositorioTipoPago;
            this.repositorioMovimiento = repositorioMovimiento;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<GetPagoDTO>>> GetAll()
        {
            try
            {
                var pagos = await repositorioPago.SelectConRelaciones();
                if (pagos == null)
                {
                    return NotFound("No se encontraron Pagos que mostrar");
                }

                var pagosDTO = mapper.Map<List<GetPagoDTO>>(pagos);
                return Ok(pagosDTO);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetAll: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetPagoDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest($"El id no puede ser '0', favor verificar ingreso de datos");

            }

            try
            {
                var pago = await repositorioPago.SelectConRelacionesXId(id);
                if (pago == null)
                {

                    return BadRequest($"No se encontro un Pago con el ID '{id}' que mostrar");

                }

                var pagoDTO = mapper.Map<GetPagoDTO>(pago);
                return Ok(pagoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpGet("Archivados")]
        public async Task<ActionResult<List<GetPagoDTO>>> GetArchivados()
        {
            try
            {
                var pagos = await repositorioPago.SelectArchivadosConRelaciones();
                if (pagos == null || !pagos.Any())
                    return NotFound("No hay pagos archivados.");

                var pagosArchivadosDTO = mapper.Map<List<GetPagoDTO>>(pagos);
                return Ok(pagosArchivadosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPost("Filtrar")]
        public async Task<ActionResult<List<GetPagoDTO>>> Filtrar([FromBody] Dictionary<string, object?> filtros)
        {
            var pagos = await repositorioPago.FiltrarConRelacionesAsync(filtros);
            var dto = mapper.Map<List<GetPagoDTO>>(pagos);
            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult<GetPagoDTO>> Post(PostPagoDTO postPagoDTO)
        {
            try
            {
                if (postPagoDTO == null)
                    return BadRequest("El cuerpo de la solicitud está vacío.");

                // ---- Validaciones de existencia ----
                var mov = await repositorioMovimiento.SelectMovimientoPorIdAsync(postPagoDTO.MovimientoId);
                if (mov == null)
                    return NotFound($"No existe un movimiento con ID {postPagoDTO.MovimientoId}.");

                var tipoPago = await repositorioTipoPago.SelectTipoPagosxId(postPagoDTO.TipoPagoId);
                if (tipoPago == null)
                    return NotFound($"No existe un tipo de pago con ID {postPagoDTO.TipoPagoId}.");

                // ---- Validación de Situación ----
                if (!Enum.IsDefined(typeof(SituacionPagoDto), postPagoDTO.Situacion))
                    return BadRequest("La situación del pago no es válida.");

                // ---- Validación comercial: monto ----
                decimal totalPagado = await repositorioPago.SelectTotalPagosPorMovimientoAsync(postPagoDTO.MovimientoId);
                decimal saldo = mov.Monto - totalPagado;

                if (postPagoDTO.Monto > saldo && !postPagoDTO.EsPagoCliente)
                {
                    return BadRequest($"El monto ({postPagoDTO.Monto}) excede el saldo restante del movimiento ({saldo}).");
                }

                // ---- Sanitizar descripción ----
                if (!string.IsNullOrWhiteSpace(postPagoDTO.Descripcion))
                    postPagoDTO.Descripcion = postPagoDTO.Descripcion.Trim();

                // ---- Mapear + Guardar ----
                var pago = mapper.Map<Pago>(postPagoDTO);

                // Convertir Situación DTO → Situación entidad
                pago.Situacion = (Situacion)postPagoDTO.Situacion;

                // Estado inicial según situación
                pago.Estado = pago.Situacion != Situacion.Completo;

                var dto = await repositorioPago.InsertDevuelveDTO<GetPagoDTO>(pago);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar un pago Metodo Post: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        // =====================================================
        // PUT ESPECIAL: SOLO CAMBIAR SITUACIÓN
        // =====================================================

        [HttpPut("{id:int}/situacion")]
        public async Task<ActionResult<GetPagoDTO>> CambiarSituacion(int id, [FromBody] PutPagoSituacionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var pago = await repositorioPago.CambiarSituacionPagoAsync(id, (Situacion)dto.NuevaSituacion, dto.Observacion);

                var dtoPago = mapper.Map<GetPagoDTO>(pago);
                return Ok(dtoPago);
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
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }



    }
}