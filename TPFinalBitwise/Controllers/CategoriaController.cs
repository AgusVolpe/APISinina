using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPFinalBitwise.DAL.Implementaciones;

namespace TPFinalBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IGenericRepository<Categoria> _repository;
        private readonly IMapper _mapper;
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaController(IGenericRepository<Categoria> repository, IMapper mapper, ICategoriaRepository categoriaRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _categoriaRepository = categoriaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObtenerTodos()
        {
            var categorias = await _repository.ObtenerTodos();
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDTO);
        }

        [HttpGet("{id}", Name = "GetCategoria")]
        public async Task<ActionResult<CategoriaDTO>> ObtenerPorId(int id)
        {
            var categoria = await _repository.ObtenerPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDTO);
           
        }

        //[ResponseCache(CacheProfileName = "PorDefecto")]
        [HttpGet("ObtenerConDataRelacionada/{id}")]
        public async Task<ActionResult<CategoriaConProductosDTO>> ObtenerConDataRelacionada(int id)
        {
            var categoria = await _categoriaRepository.ObtenerPorIdConData(id);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaConProductosDTO = _mapper.Map<CategoriaConProductosDTO>(categoria);
            return Ok(categoriaConProductosDTO);
        }
        

        //[ResponseCache(CacheProfileName = "PorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<CategoriaConProductosDTO>>> TodosConDataRelacionada()
        {
            var categorias = await _categoriaRepository.ObtenerTodosConData();
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaConProductosDTO>>(categorias);
            return Ok(categoriasDTO);
        }


        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Insertar(CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoria = _mapper.Map<Categoria>(categoriaCreacionDTO);

            var resultado = await _repository.Insertar(categoria);
            if (!resultado)
            {
                return NotFound();
            }
            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        //[Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, CategoriaCreacionDTO categoriaCreacionDTO)
        {
            var categoria = await _repository.ObtenerPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }
            _mapper.Map(categoriaCreacionDTO, categoria);
            var resultado = await _repository.Actualizar(categoria);
            if (!resultado)
            {
                return BadRequest();
            }
            return NoContent();
        }

        //[Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resultado = await _repository.Eliminar(id);
            if (!resultado)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
