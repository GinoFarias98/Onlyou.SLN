using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Pedidos.EstadoPedido;
using Onlyou.Shared.DTOS.Producto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/EstadoPedidos")]
    public class EstadoPedidosController : ControllerBase
    {
        private readonly IRepositorioEstadoPedido repositorioEstadoPedido;
        private readonly IMapper mapper;
        private readonly Context context;

        public EstadoPedidosController(Context context, IRepositorioEstadoPedido repositorioEstadoPedido, IMapper mapper)
        {
            this.context = context;
            this.repositorioEstadoPedido = repositorioEstadoPedido;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EstadoPedidoDTO>>> GetAll()
        {
            try
            {
                // CORREGIR: Cambiar el nombre de la variable y el mapeo
                var estadosPedido = await repositorioEstadoPedido.Select();

                if (estadosPedido == null || !estadosPedido.Any())
                {
                    // Cambiar el mensaje para que sea apropiado
                    return Ok(new List<EstadoPedidoDTO>()); // Retornar lista vacía en lugar de BadRequest
                }

                // CORREGIR: Mapear a EstadoPedidoDTO en lugar de GetProductoDTO
                var estadosPedidoDTO = mapper.Map<List<EstadoPedidoDTO>>(estadosPedido);

                return Ok(estadosPedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetAll de EstadoPedidos: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
        // GET: api/EstadoPedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoPedidoDTO>> GetEstadoPedido(int id)
        {
            try
            {
                var estadoPedido = await repositorioEstadoPedido.SelectById(id);

                if (estadoPedido == null)
                {
                    return NotFound($"EstadoPedido con ID {id} no encontrado");
                }

                var estadoPedidoDTO = mapper.Map<EstadoPedidoDTO>(estadoPedido);
                return Ok(estadoPedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo estado pedido {id}: {ex.Message}");
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }



        // PUT: api/EstadoPedidos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoPedido(int id, EstadoPedido estadoPedido)
        {
            if (id != estadoPedido.Id)
            {
                return BadRequest();
            }

            context.Entry(estadoPedido).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoPedidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EstadoPedidos/Inicializar
        [HttpPost("Inicializar")]
        public async Task<ActionResult> InicializarEstadosBasicos()
        {
            var estadosExistentes = await context.EstadoPedidos.AnyAsync();
            if (estadosExistentes)
            {
                return Conflict("Los estados de pedido ya están inicializados.");
            }

            var estadosBasicos = new List<EstadoPedido>
            {
                new EstadoPedido { Nombre = "Pendiente" },
                new EstadoPedido { Nombre = "Confirmado" },
                new EstadoPedido { Nombre = "En preparación" },
                new EstadoPedido { Nombre = "Listo para retirar" },
                new EstadoPedido { Nombre = "Entregado" },
                new EstadoPedido { Nombre = "Cancelado" }
            };

            await context.EstadoPedidos.AddRangeAsync(estadosBasicos);
            await context.SaveChangesAsync();

            return Ok($"Se crearon {estadosBasicos.Count} estados de pedido básicos.");
        }

        // POST: api/EstadoPedidos
        [HttpPost]
        public async Task<ActionResult<EstadoPedido>> PostEstadoPedido(EstadoPedido estadoPedido)
        {
            context.EstadoPedidos.Add(estadoPedido);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoPedido", new { id = estadoPedido.Id }, estadoPedido);
        }

        // DELETE: api/EstadoPedidos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstadoPedido(int id)
        {
            var estadoPedido = await context.EstadoPedidos.FindAsync(id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

           context.EstadoPedidos.Remove(estadoPedido);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoPedidoExists(int id)
        {
            return context.EstadoPedidos.Any(e => e.Id == id);
        }
    }
}