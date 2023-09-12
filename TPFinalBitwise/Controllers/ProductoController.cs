using AutoMapper;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TPFinalBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IGenericRepository<Producto> _repository;
        private readonly IMapper _mapper;
        private readonly IProductoRepository _productoRepository;

        public ProductoController(IGenericRepository<Producto> repository, IMapper mapper, IProductoRepository productoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _productoRepository = productoRepository;
        }

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> ObtenerTodos()
        {
            var productos = await _repository.ObtenerTodos();
            var productosDTO = _mapper.Map<IEnumerable<ProductoDTO>>(productos);
            return Ok(productosDTO);
        }

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("{id}", Name = "GetProducto")]
        public async Task<ActionResult<ProductoDTO>> ObtenerPorId(int id)
        {
            var producto = await _repository.ObtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            var productoDTO = _mapper.Map<ProductoDTO>(producto);
            return Ok(productoDTO);
        }

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("ObtenerConDataRelacionada/{id}")]
        public async Task<ActionResult<ProductoDTO>> ObtenerConDataRelacionada(int id)
        {
            var producto = await _productoRepository.ObtenerPorIdConData(id);
            if (producto == null)
            {
                return NotFound();
            }
            var productoDTO = _mapper.Map<ProductoDTO>(producto);
            return Ok(productoDTO);
        }
        
        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> TodosConDataRelacionada()
        {
            var productos = await _productoRepository.ObtenerTodosConData();           
            var productosDTO = _mapper.Map<IEnumerable<ProductoDTO>>(productos);
            return Ok(productosDTO);
        }
        

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Insertar(ProductoCreacionDTO productoCreacionDTO)
        {
            var producto = _mapper.Map<Producto>(productoCreacionDTO);

            var resultado = await _repository.Insertar(producto);
            if (!resultado)
            {
                return NotFound();
            }
            var productoDTO = _mapper.Map<ProductoDTO>(producto);

            return Ok(productoDTO);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, ProductoCreacionDTO productoCreacionDTO)
        {
            var producto = await _repository.ObtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            _mapper.Map(productoCreacionDTO, producto);
            var resultado = await _repository.Actualizar(producto);
            if (!resultado)
            {
                return BadRequest();
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
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
