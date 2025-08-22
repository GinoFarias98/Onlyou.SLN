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

        // GET: api/talles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TallesDTO>>> GetAll()
        {
            var talles = await repositorio.Select();
            var tallesDTO = mapper.Map<List<TallesDTO>>(talles);
            return Ok(tallesDTO);
        }

        // GET: api/talles/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TallesDTO>> GetById(int id)
        {
            var talle = await repositorio.SelectById(id);
            if (talle == null)
                return NotFound();

            var talleDTO = mapper.Map<TallesDTO>(talle);
            return Ok(talleDTO);
        }

        // GET: api/talles/codigo/ABC123
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<TallesDTO>> GetByCodigo(string codigo)
        {
            var talle = await repositorio.SelectByCod(codigo);
            if (talle == null)
                return NotFound();

            var talleDTO = mapper.Map<TallesDTO>(talle);
            return Ok(talleDTO);
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

        // POST: api/talles
        [HttpPost]
        public async Task<ActionResult<TallesDTO>> Create([FromBody] TallesDTO talleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var talle = mapper.Map<Talle>(talleDTO);
            var id = await repositorio.Insert(talle);

            // Devolvemos el DTO creado
            var creadoDTO = mapper.Map<TallesDTO>(talle);
            return CreatedAtAction(nameof(GetById), new { id = id }, creadoDTO);
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

        // PATCH: api/talles/estado/5
        [HttpPatch("estado/{id:int}")]
        public async Task<IActionResult> ToggleEstado(int id, [FromBody] TallesDTO talleDTO)
        {
            var talle = mapper.Map<Talle>(talleDTO);
            var updated = await repositorio.UpdateEstado(id, talle);
            if (!updated)
                return NotFound();

            return NoContent();
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
