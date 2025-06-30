using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Onlyou.BD.Data.Entidades;
using Onlyou.Server.Repositorio;
using Onlyou.Shared.DTOS;
using Onlyou.Shared.DTOS.Categorias;

namespace Onlyou.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IRepositorio<Categoria> repositorio;
        private readonly IMapper mapper;

        public CategoriasController(IRepositorio<Categoria> repositorio, IMapper mapper)
        {
            this.repositorio = repositorio;
            this.mapper = mapper;
        }

        // GET: api/categorias
        [HttpGet]
        public async Task<ActionResult<List<CrearCategoriasDTO>>> GetAll()
        {
            var categorias = await repositorio.Select();
            var dto = mapper.Map<List<CrearCategoriasDTO>>(categorias);
            return Ok(dto);
        }

        // GET: api/categorias/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CrearCategoriasDTO>> Get(int id)
        {
            var categoria = await repositorio.SelectById(id);
            if (categoria == null) return NotFound();
            var dto = mapper.Map<CrearCategoriasDTO>(categoria);
            return Ok(dto);
        }

        // POST: api/categorias
        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCategoriasDTO dto)
        {
            var entidad = mapper.Map<Categoria>(dto);
            var id = await repositorio.Insert(entidad);
            return Ok(id);
        }

        //PUT: api/categorias/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, EditarCategoriasDTO categoriaDTO)
        {
            if (id != categoriaDTO.Id)
                return BadRequest("IDs no coinciden");

            var existente = await repositorio.SelectById(id);
            if (existente == null)
                return NotFound("No existe la categoría");

            if (string.IsNullOrWhiteSpace(categoriaDTO.Nombre))
                return BadRequest("El nombre de la categoría es obligatorio.");

            // Mapeamos SOLO los datos actualizados sobre la entidad existente
            mapper.Map(categoriaDTO, existente);

            var ok = await repositorio.UpdateEntidad(id, existente);
            return ok ? Ok() : BadRequest();
        }


        // DELETE: api/categorias/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await repositorio.Existe(id);
            if (!existe) return NotFound();

            var ok = await repositorio.Delete(id);
            return ok ? Ok() : BadRequest();
        }
    }
}

