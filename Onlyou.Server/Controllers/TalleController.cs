using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS.Talle;



namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalleController : ControllerBase
    {
        private readonly IRepositorioTalle repositorio;
        private readonly IMapper mapper;

        public TalleController(IRepositorioTalle repositorio, IMapper mapper)
        {
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TallesDTO>>> GetAll()
        {
            var talles = await repositorio.Select();
            if (talles== null)
            {
                return Ok(new List<TallesDTO>());

            }
            var tallesDTO = mapper.Map<List<TallesDTO>>(talles);

            return Ok(tallesDTO);
        }

        [HttpGet("Id/{id:int}")]
        public async Task<ActionResult<TallesDTO>> GetById(int id)
        {
            try
            {
                var talle = await repositorio.SelectById(id);
                if (talle == null)
                {
                    return BadRequest($"No se encontro un talle con el ID: '{id}' que mostrar");
                }
                var talleDTO = mapper.Map<TallesDTO>(talle);
                return Ok(talleDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el método GetById: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        // GET: api/talles/producto/3
        [HttpGet("producto/{productoId:int}")]
        public async Task<ActionResult<IEnumerable<TallesDTO>>> GetByProducto(int productoId)
        {
            var talles = await repositorio.SelectTallePorProducto(productoId);
            if (talles == null || !talles.Any())
                return NotFound($"No se encontraron talles para el producto con ID {productoId}");

            var tallesDTO = mapper.Map<List<TallesDTO>>(talles);
            return Ok(tallesDTO);
        }


        [HttpGet("Archivados")]
        public async Task<ActionResult<List<TallesDTO>>> GetArchivados()
        {
            try
            {
                var talles = await repositorio.SelectArchivados();
                if (talles == null || !talles.Any())
                    return NotFound("No hay productos archivados.");

                var tallesArchivadasDTO = mapper.Map<List<TallesDTO>>(talles);
                return Ok(tallesArchivadasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetArchivados: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }



        // POST: api/talles
        [HttpPost]
        public async Task<ActionResult<TallesDTO>> Create([FromBody] TallesDTO talleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // 🔹 Llamamos al método genérico ya sobrescrito en el repo de Talle
                var creadoDTO = await repositorio.InsertDevuelveDTO<TallesDTO>(
                    mapper.Map<Talle>(talleDTO)
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


        // PUT: api/talles/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TallesDTO talleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var talle = mapper.Map<Talle>(talleDTO);
            var updated = await repositorio.UpdateEntidad(id, talle);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        //// PATCH: api/talles/estado/5
        //[HttpPatch("estado/{id:int}")]
        //public async Task<IActionResult> ToggleEstado(int id, [FromBody] TallesDTO talleDTO)
        //{
        //    var talle = mapper.Map<Talle>(talleDTO);
        //    var updated = await repositorio.UpdateEstado(id, talle);
        //    if (!updated)
        //        return NotFound();

        //    return NoContent();
        //}


        [HttpPut("Archivados/{id}")]
        public async Task<ActionResult<bool>> BajaLogica(int id)
        {
            try
            {
                var resultado = await repositorio.UpdateEstado(id);
                return resultado ? Ok(true) : NotFound(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Put Archivar: {ex.Message}");
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");

            }
        }


        // DELETE: api/talles/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await repositorio.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }


    }
}
