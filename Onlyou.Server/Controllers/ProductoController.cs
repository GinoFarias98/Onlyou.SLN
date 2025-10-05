using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onlyou.BD.Data;
using Onlyou.BD.Data.Entidades;
using Onlyou.Client.Pages.Marca;
using Onlyou.Server.Helpers;
using Onlyou.Server.Repositorio;
using Onlyou.Server.Services;
using Onlyou.Shared.DTOS.Marca;
using Onlyou.Shared.DTOS.Producto;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("Productos")]
    public class ProductoController : ControllerBase
    {
        private readonly IRepositorioProducto repositorioProducto;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly Context context;
        private readonly string contenedor = "productos";

        public ProductoController(IRepositorioProducto repositorioProducto, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos, Context context)
        {
            this.repositorioProducto = repositorioProducto;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetProductoDTO>>> GetAll()
        {
            try
            {
                var productos = await repositorioProducto.SelectConRelaciones();


                if (productos == null)
                {
                    return BadRequest("No se encontraron Productos que mostrar");
                }

                var productosDTO = mapper.Map<List<GetProductoDTO>>(productos);

                return Ok(productosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetAll: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Codigo/{codigo}")]
        public async Task<ActionResult<GetProductoDTO>> GetByCodigo(string codigo)
        {
            try
            {
                var producto = await repositorioProducto.SelectByCod(codigo);

                if (producto == null)
                {
                    return BadRequest($"No se encontro un Producto con el CODIGO '{codigo}' que mostrar");
                }
                var productoDTO = mapper.Map<GetProductoDTO>(producto);
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetByCodigo: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        [HttpGet("Id/{id}")]
        public async Task<ActionResult<GetProductoDTO>> GetById(int id)
        {
            if (id == 0)
            {
                return BadRequest($"El id no puede ser '0', favor verificar ingreso de datos");

            }

            try
            {
                var producto = await repositorioProducto.SelectConRelacionesXId(id);
                if (producto == null)
                {

                    return BadRequest($"No se encontro un Producto con el ID '{id}' que mostrar");

                }

                var productoDTO = mapper.Map<GetProductoDTO>(producto);
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<GetProductoDTO>> Post(PostProductoDTO postProductoDTO, [FromServices] IImagenValidator validator)
        {
            try
            {
                if (postProductoDTO == null)
                    return BadRequest("Favor de verificar, valor ingresado nulo.");

                // Validar y guardar imagen solo si existe
                if (!string.IsNullOrWhiteSpace(postProductoDTO.Imagen) &&
                    !string.IsNullOrWhiteSpace(postProductoDTO.ImagenExtension))
                {
                    if (!validator.ValidarBase64Extencion(postProductoDTO.Imagen, postProductoDTO.ImagenExtension))
                        return BadRequest("El contenido de la imagen no coincide con la extensión indicada.");

                    var imgProducto = Convert.FromBase64String(postProductoDTO.Imagen);

                    postProductoDTO.Imagen = await almacenadorArchivos.GuardarArchivo(
                        imgProducto,
                        postProductoDTO.ImagenExtension,
                        contenedor
                    );
                }

                // Mapear a entidad y persistir
                var producto = mapper.Map<Producto>(postProductoDTO);
                var dto = await repositorioProducto.InsertDevuelveDTO<GetProductoDTO>(producto);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Post al querer guardar un producto: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }


        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(int id, [FromBody] PutProductoDTO putProductoDTO, [FromServices] IImagenValidator validator)
        //{
        //    if (putProductoDTO == null)
        //        return BadRequest("Datos inválidos.");

        //    // Validamos que el producto exista
        //    var productoDB = await repositorioProducto.SelectById(id);
        //    if (productoDB == null)
        //        return NotFound($"No se encontró el producto con ID {id}");

        //    try
        //    {
        //        // 🖼️ Manejo de imagen
        //        if (!string.IsNullOrWhiteSpace(putProductoDTO.Imagen))
        //        {
        //            if (!validator.ValidarBase64Extencion(putProductoDTO.Imagen, putProductoDTO.ImagenExtension))
        //                return BadRequest("El contenido de la imagen no coincide con la extensión indicada.");

        //            var imgProducto = Convert.FromBase64String(putProductoDTO.Imagen);

        //            if (!string.IsNullOrEmpty(productoDB.Imagen))
        //                await almacenadorArchivos.EliminarArchivo(productoDB.Imagen, contenedor);

        //            productoDB.Imagen = await almacenadorArchivos.GuardarArchivo(
        //                imgProducto,
        //                putProductoDTO.ImagenExtension,
        //                contenedor
        //            );
        //        }

        //        // 🔄 Mapear datos básicos
        //        mapper.Map(putProductoDTO, productoDB);

        //        // 🔄 Actualizar relaciones (colores y talles)
        //        productoDB.ProductosColores.Clear();
        //        foreach (var colorId in putProductoDTO.Colores)
        //        {
        //            productoDB.ProductosColores.Add(new ProductoColor { ProductoId = id, ColorId = colorId });
        //        }

        //        productoDB.ProductosTalles.Clear();
        //        foreach (var talleId in putProductoDTO.Talles)
        //        {
        //            productoDB.ProductosTalles.Add(new ProductoTalle { ProductoId = id, TalleId = talleId });
        //        }

        //        productoDB.FecUltimaModificacion = DateTime.UtcNow;

        //        await repositorioProducto.UpdateEntidad(id, productoDB);

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error en PUT de Producto: {ex.Message}");
        //        return StatusCode(500, "Error interno al actualizar el producto.");
        //    }
        //}

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GetProductoDTO>> Put(int id, [FromBody] PutProductoDTO putProductoDTO, [FromServices] IImagenValidator validator)
        {
            if (putProductoDTO == null)
                return BadRequest("Datos inválidos.");

            var productoDB = await context.Productos
                .Include(p => p.ProductosColores)
                .Include(p => p.ProductosTalles)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (productoDB == null)
                return NotFound($"No se encontró el producto con ID {id}");

            try
            {
                // 🖼️ 1️⃣ Manejo de imagen
                if (!string.IsNullOrWhiteSpace(putProductoDTO.Imagen))
                {
                    if (!validator.ValidarBase64Extencion(putProductoDTO.Imagen, putProductoDTO.ImagenExtension))
                        return BadRequest("El contenido de la imagen no coincide con la extensión indicada.");

                    var imgProducto = Convert.FromBase64String(putProductoDTO.Imagen);

                    // Si ya existía una imagen, la eliminamos
                    if (!string.IsNullOrEmpty(productoDB.Imagen))
                        await almacenadorArchivos.EliminarArchivo(productoDB.Imagen, contenedor);

                    // Guardamos la nueva imagen
                    productoDB.Imagen = await almacenadorArchivos.GuardarArchivo(
                        imgProducto,
                        putProductoDTO.ImagenExtension,
                        contenedor
                    );
                }

                // 🔄 2️⃣ Aplicamos AutoMapper a los campos simples
                mapper.Map(putProductoDTO, productoDB);

                // 🔁 3️⃣ Actualizamos relaciones (colores y talles)
                productoDB.ProductosColores.Clear();
                foreach (var colorId in putProductoDTO.Colores)
                {
                    productoDB.ProductosColores.Add(new ProductoColor
                    {
                        ProductoId = productoDB.Id,
                        ColorId = colorId
                    });
                }

                productoDB.ProductosTalles.Clear();
                foreach (var talleId in putProductoDTO.Talles)
                {
                    productoDB.ProductosTalles.Add(new ProductoTalle
                    {
                        ProductoId = productoDB.Id,
                        TalleId = talleId
                    });
                }

                // 🕓 4️⃣ Actualizamos fecha de modificación
                productoDB.FecUltimaModificacion = DateTime.UtcNow;

                // 💾 5️⃣ Guardamos cambios
                await context.SaveChangesAsync();


                var productoDTO = mapper.Map<GetProductoDTO>(productoDB);

                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en PUT de Producto: {ex.Message}");
                return StatusCode(500, "Error interno al actualizar el producto.");
            }
        }


        // =============================================================


        [HttpDelete("EliminarProducto/{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            try
            {
                var entidad = await repositorioProducto.SelectById(id);

                if (entidad == null)
                {
                    return NotFound($"No se encontró un Producto con Id {id}. Favor verificar");
                }

                var eliminado = await repositorioProducto.Delete(entidad.Id);

                if (eliminado)
                {
                    return Ok($"El Producto con Id {id} fue eliminado");
                }

                return BadRequest("No se pudo llevar a cabo la acción");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método Delete: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }

        }

    }
}
