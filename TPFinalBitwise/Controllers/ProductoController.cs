using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Models;
using TPFinalBitwise.DTO;
using Microsoft.EntityFrameworkCore.Metadata;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> ObtenerTodos()
        {
            var productos = await _repository.ObtenerTodos();
            var productosDTO = _mapper.Map<IEnumerable<ProductoDTO>>(productos);
            return Ok(productosDTO);
        }

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

        //[ResponseCache(CacheProfileName = "PorDefecto")]
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
        
        //[ResponseCache(CacheProfileName = "PorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> TodosConDataRelacionada()
        {
            var productos = await _productoRepository.ObtenerTodosConData();           
            var productosDTO = _mapper.Map<IEnumerable<ProductoDTO>>(productos);
            return Ok(productosDTO);
        }
        

        //[Authorize(Roles = "admin")]
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

        //[Authorize(Roles = "admin")]
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
