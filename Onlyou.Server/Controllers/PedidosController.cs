using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Pedidos;
using Onlyou.Shared.DTOS.Pedidos.EstadoPedido;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/Pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IRepositorioPedido _repoPedido;
        private readonly IRepositorioPedidoItem _repoPedidoItem;
        private readonly IRepositorioEstadoPedido _repoEstadoPedido;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public PedidosController(
            IRepositorioPedido repoPedido,
            IRepositorioPedidoItem repoPedidoItem,
            IRepositorioEstadoPedido repoEstadoPedido,
            IMapper mapper,
            Context context)
        {
            _repoPedido = repoPedido;
            _repoPedidoItem = repoPedidoItem;
            _repoEstadoPedido = repoEstadoPedido;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet]
        public async Task<ActionResult<List<GetPedidosDTO>>> GetAll()
        {
            try
            {
                var pedidos = await _context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Producto)
                    .OrderByDescending(p => p.FechaGenerado)
                    .ToListAsync();

                if (pedidos == null || !pedidos.Any())
                {
                    return Ok(new List<GetPedidosDTO>());
                }

                var pedidosDTO = _mapper.Map<List<GetPedidosDTO>>(pedidos);
                return Ok(pedidosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetAll: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/Pedidos/Estados
        [HttpGet("Estados")]
        public async Task<ActionResult<List<EstadoPedidoDTO>>> GetEstados()
        {
            try
            {
                var estados = await _context.EstadoPedidos.ToListAsync();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetEstados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/Pedidos/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetPedidosDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest("El id no puede ser '0'");
            }

            try
            {
                var pedido = await _repoPedido.SelectPedidoPorIdAsync(id);
                if (pedido == null)
                {
                    return NotFound($"No se encontró un Pedido con el ID '{id}'");
                }

                var pedidoDTO = _mapper.Map<GetPedidosDTO>(pedido);
                return Ok(pedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<GetPedidosDTO>> Post(PostPedidoDTO postPedidoDTO)
        {
            try
            {
                if (postPedidoDTO == null)
                    return BadRequest("Favor de verificar, valor ingresado nulo.");

                // Validar que el EstadoPedidoId existe
                var estadoPedido = await _context.EstadoPedidos.FindAsync(postPedidoDTO.EstadoPedidoId);
                if (estadoPedido == null)
                {
                    return BadRequest("El EstadoPedidoId especificado no existe");
                }

                // Validar que los Productos existen
                foreach (var item in postPedidoDTO.PedidoItems)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto == null)
                    {
                        return BadRequest($"El ProductoId {item.ProductoId} no existe");
                    }
                }

                // Mapear el DTO a la entidad Pedido
                var pedido = _mapper.Map<Pedido>(postPedidoDTO);

                // Establecer valores por defecto
                pedido.FechaGenerado = DateTime.UtcNow;
                pedido.FechaPedidoAProveedor = DateTime.UtcNow;
                pedido.EstadoEntrega = EstadoEntrega.NoEntregado;
                pedido.EstadoPago = EstadoPago.NoPagado;
                pedido.MontoEntregado = 0;
                pedido.MontoPagado = 0;

                // Insertar el pedido
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Cargar relaciones para el DTO
                var pedidoConRelaciones = await _context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Producto)
                    .FirstOrDefaultAsync(p => p.Id == pedido.Id);

                var dto = _mapper.Map<GetPedidosDTO>(pedidoConRelaciones);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // PUT: api/Pedidos/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GetPedidosDTO>> Put(int id, PutPedidoDTO putPedidoDTO)
        {
            if (putPedidoDTO == null)
                return BadRequest("Datos inválidos.");

            var pedidoDB = await _context.Pedidos
                .Include(p => p.PedidoItems)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedidoDB == null)
                return NotFound($"No se encontró el pedido con ID {id}");

            try
            {
                _mapper.Map(putPedidoDTO, pedidoDB);
                await _context.SaveChangesAsync();

                var pedidoActualizado = await _repoPedido.SelectPedidoPorIdAsync(id);
                var pedidoDTO = _mapper.Map<GetPedidosDTO>(pedidoActualizado);

                return Ok(pedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PUT de Pedido: {ex.Message}");
                return StatusCode(500, "Error interno al actualizar el pedido.");
            }
        }

        // PATCH: api/Pedidos/MarcarProveedor/{id}
        [HttpPatch("MarcarProveedor/{id:int}")]
        public async Task<IActionResult> MarcarComoPedidoProveedor(int id)
        {
            try
            {
                var pedido = await _context.Pedidos.FindAsync(id);

                if (pedido == null)
                    return NotFound($"No se encontró el pedido con ID {id}");

                pedido.FechaPedidoAProveedor = DateTime.Now;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Id = id,
                    FechaPedidoAProveedor = pedido.FechaPedidoAProveedor,
                    Mensaje = "Pedido marcado como enviado al proveedor"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en MarcarComoPedidoProveedor: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // DELETE: api/Pedidos/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var entidad = await _repoPedido.SelectPedidoPorIdAsync(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Pedido con Id {id}");
                }

                _context.Pedidos.Remove(entidad);
                await _context.SaveChangesAsync();

                return Ok($"El Pedido con Id {id} fue eliminado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }
    }
}