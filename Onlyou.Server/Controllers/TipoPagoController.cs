using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.Producto;
using Onlyou.Shared.DTOS.Proveedor;
using Onlyou.Shared.DTOS.Talle;
using Onlyou.Shared.DTOS.TipoPago;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("/TipoPago")]
    public class TipoPagoController :ControllerBase
    {
        private readonly IRepositorioTipoPago repositorioTipoPago;
        private readonly IMapper mapper;

        public TipoPagoController(IRepositorioTipoPago repositorioTipoPago, IMapper mapper)
        {
            this.repositorioTipoPago = repositorioTipoPago;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<TipoPagoDTO>>> GetAll()
        {
            try
            {
                var tipoPagos = await repositorioTipoPago.Select();


                if (tipoPagos == null)
                {
                    return NotFound("No se encontraron Tipo de Pagos que mostrar");
                }

                var productosDTO = mapper.Map<List<TipoPagoDTO>>(tipoPagos);

                return Ok(productosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetAll: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Id/{id}")]
        public async Task<ActionResult<TipoPagoDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest($"El id no puede ser '0', favor verificar ingreso de datos");

            }

            try
            {
                var tipoPago = await repositorioTipoPago.SelectById(id);
                if (tipoPago == null)
                {

                    return NotFound($"No se encontro un Tipo Pago con el ID '{id}' que mostrar");

                }

                var tPagoDTO = mapper.Map<TipoPagoDTO>(tipoPago);
                return Ok(tPagoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<TipoPagoDTO>>> GetArchivados()
        {
            try
            {
                var tipoPago = await repositorioTipoPago.SelectArchivados();
                if (tipoPago == null || !tipoPago.Any())
                    return NotFound("No hay Tipo de Pagos archivados.");

                var tipoPagoArchivadosDTO = mapper.Map<List<TipoPagoDTO>>(tipoPago);
                return Ok(tipoPagoArchivadosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPost("Filtrar")]
        public async Task<ActionResult<List<TipoPagoDTO>>> Filtrar([FromBody] Dictionary<string, object?> filtros)
        {
            var productos = await repositorioTipoPago.FiltrarAsync(filtros);
            var dto = mapper.Map<List<TipoPagoDTO>>(productos);
            return Ok(dto);
        }


        [HttpPost]
        public async Task<ActionResult<TipoPagoDTO>> Create([FromBody] TipoPagoDTO tipoPagoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 🔹 Llamamos al método genérico ya sobrescrito en el repo de Talle
                var creadoDTO = await repositorioTipoPago.InsertDevuelveDTO<TipoPagoDTO>(
                    mapper.Map<TipoPago>(tipoPagoDTO)
                );

                return CreatedAtAction(nameof(GetById), new { id = creadoDTO.Id }, creadoDTO);
            }
            catch (InvalidOperationException ex)
            {
                // ⚠️ Duplicado controlado
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Create: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPut("ModificarXId/{id}")]
        public async Task<ActionResult<TipoPagoDTO>> Put(int id, [FromBody] TipoPagoDTO tipoPagoDTO)
        {
            try
            {
                var tipoPago = await repositorioTipoPago.SelectById(id);
                if (tipoPago == null)
                {
                    return BadRequest($"No se encontró un tipo de pago con Id {id}.");
                }
                // Mapear los cambios del DTO al color
                mapper.Map(tipoPagoDTO, tipoPago);

                // Actualizar en la base de datos
                await repositorioTipoPago.UpdateEntidad(id, tipoPago);

                // Devolver un DTO de lectura para mostrar en el frontend
                var TipoPagoDto = mapper.Map<TipoPagoDTO>(tipoPago);

                return Ok(TipoPagoDto); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el metodo Put: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorioTipoPago.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }


        [HttpDelete("EliminarProveedor/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                await repositorioTipoPago.EliminarTipoPagoAsync(id);
                return Ok(new { mensaje = "Proveedor eliminado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

    }
}
