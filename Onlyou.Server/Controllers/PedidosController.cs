using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Pedidos;
using Onlyou.Shared.DTOS.Pedidos.EstadoPedido;
using Onlyou.Shared.DTOS.Pedidos.PedidoItem;

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
                var pedidos = await _repoPedido.GetAllWithDetailsAsync();

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
                var estados = await _repoEstadoPedido.Select();
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
            if (id == 0) return BadRequest("El id no puede ser '0'");

            try
            {
                var pedido = await _repoPedido.SelectPedidoPorIdAsync(id);
                if (pedido == null) return NotFound($"No se encontró un Pedido con el ID '{id}'");

                // DEBUG: Verificar datos ANTES del mapeo
                Console.WriteLine("🔍 DEBUG - Datos ANTES del mapeo:");
                foreach (var item in pedido.PedidoItems)
                {
                    Console.WriteLine($"   Producto: {item.Producto?.Nombre}");
                    Console.WriteLine($"   Proveedor: {item.Producto?.Proveedor?.Nombre}");
                    Console.WriteLine($"   ProveedorId: {item.Producto?.ProveedorId}");
                    Console.WriteLine($"   Proveedor es null: {item.Producto?.Proveedor == null}");
                }

                var pedidoDTO = _mapper.Map<GetPedidosDTO>(pedido);

                // DEBUG: Verificar datos DESPUÉS del mapeo
                Console.WriteLine("🔍 DEBUG - Datos DESPUÉS del mapeo:");
                foreach (var item in pedidoDTO.PedidoItems)
                {
                    Console.WriteLine($"   Producto: {item.ProductoNombre}");
                    Console.WriteLine($"   Proveedor: {item.ProveedorNombre}");
                    Console.WriteLine($"   ProveedorId: {item.ProveedorId}");
                }

                return Ok(pedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetById: {ex.Message}");
                return StatusCode(500, $"Error interno: {ex.Message}");
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
                var estadoPedido = await _repoEstadoPedido.SelectById(postPedidoDTO.EstadoPedidoId);
                if (estadoPedido == null)
                {
                    return BadRequest("El EstadoPedidoId especificado no existe");
                }

                // Validar productos y cargar datos de proveedores
                foreach (var item in postPedidoDTO.PedidoItems)
                {
                    Console.WriteLine($"🔍 ITEM RECIBIDO: ProductoId={item.ProductoId}, ColorId={item.ColorId}, TalleId={item.TalleId}");
                    var producto = await _context.Productos
                        .Include(p => p.Proveedor) // INCLUIR PROVEEDOR
                        .FirstOrDefaultAsync(p => p.Id == item.ProductoId);

                    if (producto == null)
                    {
                        return BadRequest($"El ProductoId {item.ProductoId} no existe");
                    }
                }

                // Mapear el DTO a la entidad Pedido
                var pedido = _mapper.Map<Pedido>(postPedidoDTO);

                // Establecer valores por defecto
                pedido.FechaGenerado = DateTime.Now;
                pedido.FechaPedidoAProveedor = DateTime.MinValue;
                pedido.EstadoEntrega = EstadoEntrega.NoEntregado;
                pedido.EstadoPago = EstadoPago.NoPagado;
                pedido.MontoEntregado = 0;
                pedido.MontoPagado = 0;

                // Guardar en base de datos
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Pedido creado con ID: {pedido.Id}");

                // Cargar relaciones para el DTO - INCLUYENDO
                var pedidoConRelaciones = await _context.Pedidos
                     .Include(p => p.EstadoPedido)
                     .Include(p => p.PedidoItems)
                         .ThenInclude(pi => pi.Producto)
                             .ThenInclude(producto => producto.Proveedor)
                     .Include(p => p.PedidoItems)
                         .ThenInclude(pi => pi.Color)
                     .Include(p => p.PedidoItems)
                         .ThenInclude(pi => pi.Talle)
                     .FirstOrDefaultAsync(p => p.Id == pedido.Id);

                if (pedidoConRelaciones == null)
                {
                    return StatusCode(500, "Pedido creado pero no se pudo cargar para respuesta");
                }

                var dto = _mapper.Map<GetPedidosDTO>(pedidoConRelaciones);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error crítico en el método Post: {ex.Message}");
                Console.WriteLine($"🔍 StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // PUT: api/Pedidos/{id} - CORREGIDO
        [HttpPut("{id}")]
        public async Task<ActionResult<GetPedidosDTO>> Put(int id, PutPedidoDTO putPedidoDTO)
        {
            try
            {
                Console.WriteLine($"📝 Actualizando pedido {id}");

                // 1. Obtener pedido con ítems + relaciones
                var pedidoExistente = await _context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (pedidoExistente == null)
                {
                    return NotFound($"Pedido con ID {id} no encontrado");
                }

                // 2. Actualizar propiedades del pedido
                pedidoExistente.MontoTotal = putPedidoDTO.MontoTotal;
                pedidoExistente.Descripcion = putPedidoDTO.Descripcion;
                pedidoExistente.NombreCliente = putPedidoDTO.NombreCliente;
                pedidoExistente.DireccionCliente = putPedidoDTO.DireccionCliente;
                pedidoExistente.Localidad = putPedidoDTO.Localidad;
                pedidoExistente.DNI = putPedidoDTO.DNI;
                pedidoExistente.Telefono = putPedidoDTO.Telefono;
                pedidoExistente.EstadoPedidoId = putPedidoDTO.EstadoPedidoId;
                pedidoExistente.EstadoEntrega = (EstadoEntrega)putPedidoDTO.EstadoEntrega;
                pedidoExistente.EstadoPago = (EstadoPago)putPedidoDTO.EstadoPago;
                pedidoExistente.MontoPagado = putPedidoDTO.MontoPagado;

                Console.WriteLine("🔄 Editando ítems del pedido…");

                // 3. Actualizar cada item
                foreach (var itemDTO in putPedidoDTO.PedidoItems)
                {
                    var itemExistente = pedidoExistente.PedidoItems
                        .FirstOrDefault(pi => pi.Id == itemDTO.Id);

                    if (itemExistente != null)
                    {
                        itemExistente.Cantidad = itemDTO.Cantidad;
                        itemExistente.PrecioUnitarioVenta = itemDTO.PrecioUnitarioVenta;

                        itemExistente.ColorId = itemDTO.ColorId;
                        itemExistente.ColorNombre = itemDTO.ColorNombre;

                        itemExistente.TalleId = itemDTO.TalleId;
                        itemExistente.TalleNombre = itemDTO.TalleNombre;

                        Console.WriteLine($"✔ Item {itemExistente.Id} actualizado");
                    }
                    else
                    {
                        Console.WriteLine($"⚠ Item {itemDTO.Id} no existe en el pedido");
                    }
                }

                // 4. Guardar cambios
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ Pedido {id} e ítems actualizados correctamente");

                // 5. Recargar pedido para devolver DTO completo
                var pedidoActualizado = await _context.Pedidos
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Producto)
                            .ThenInclude(prod => prod.Proveedor)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Color)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(pi => pi.Talle)
                    .FirstOrDefaultAsync(p => p.Id == id);

                var pedidoDTO = _mapper.Map<GetPedidosDTO>(pedidoActualizado);
                return Ok(pedidoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en PUT Pedido: {ex.Message}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PATCH: api/Pedidos/MarcarProveedor/{id} - CORREGIDO
        [HttpPatch("MarcarProveedor/{id:int}")]
        public async Task<IActionResult> MarcarComoPedidoProveedor(int id)
        {
            try
            {
                var pedido = await _repoPedido.SelectById(id);

                if (pedido == null)
                    return NotFound($"No se encontró el pedido con ID {id}");

                pedido.FechaPedidoAProveedor = DateTime.Now;

                // USAR EL MÉTODO CORRECTO
                await _repoPedido.UpdateAsync(pedido);

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

        // PATCH: api/Pedidos/CambiarEstado/{id} - CORREGIDO
        [HttpPatch("CambiarEstado/{id:int}")]
        public async Task<IActionResult> CambiarEstadoPedido(int id, [FromBody] CambiarEstadoDTO cambiarEstadoDTO)
        {
            try
            {
                var pedido = await _repoPedido.SelectById(id);

                if (pedido == null)
                    return NotFound($"No se encontró el pedido con ID {id}");

                // Validar que el estado exista
                var estadoExiste = await _repoEstadoPedido.Existe(cambiarEstadoDTO.EstadoPedidoId);
                if (!estadoExiste)
                    return BadRequest("El estado de pedido especificado no existe");

                pedido.EstadoPedidoId = cambiarEstadoDTO.EstadoPedidoId;

                // USAR EL MÉTODO CORRECTO
                await _repoPedido.UpdateAsync(pedido);

                return Ok(new
                {
                    Id = id,
                    EstadoPedidoId = pedido.EstadoPedidoId,
                    Mensaje = "Estado del pedido actualizado correctamente"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CambiarEstadoPedido: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPatch("Item/{itemId:int}")]
        public async Task<IActionResult> PatchPedidoItem(int itemId, [FromBody] PatchPedidoItemDTO dto)
        {
            var pedidoItem = await _context.PedidoItems
                .FirstOrDefaultAsync(pi => pi.Id == itemId);

            if (pedidoItem == null)
                return NotFound("El item del pedido no existe");

            bool hizoCambios = false;

            if (dto.ColorId.HasValue)
            {
                pedidoItem.ColorId = dto.ColorId.Value;
                _context.Entry(pedidoItem).Property(p => p.ColorId).IsModified = true;
                hizoCambios = true;
            }

            if (dto.TalleId.HasValue)
            {
                pedidoItem.TalleId = dto.TalleId.Value;
                _context.Entry(pedidoItem).Property(p => p.TalleId).IsModified = true;
                hizoCambios = true;
            }

            if (!hizoCambios)
                return Ok(new { Mensaje = "Sin cambios" });

            await _context.SaveChangesAsync();

            // Recargar solo los nombres
            await _context.Entry(pedidoItem).Reference(p => p.Color).LoadAsync();
            await _context.Entry(pedidoItem).Reference(p => p.Talle).LoadAsync();

            return Ok(new
            {
                Mensaje = "Item actualizado",
                ColorId = pedidoItem.ColorId,
                ColorNombre = pedidoItem.Color?.Nombre,
                TalleId = pedidoItem.TalleId,
                TalleNombre = pedidoItem.Talle?.Nombre
            });
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

                await _repoPedido.Delete(id);

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