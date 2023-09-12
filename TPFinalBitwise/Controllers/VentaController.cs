using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IGenericRepository<Venta> _repository;
        private readonly IMapper _mapper;
        private readonly IVentaRepository _ventaRepository;

        public VentaController(IGenericRepository<Venta> repository, IMapper mapper, IVentaRepository ventaRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _ventaRepository = ventaRepository;
        }

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> ObtenerTodos()
        {
            var ventas = await _repository.ObtenerTodos();
            var ventasDTO = _mapper.Map<IEnumerable<VentaDTO>>(ventas);
            return Ok(ventasDTO);
        }

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("{id}", Name = "GetVenta")]
        public async Task<ActionResult<VentaDTO>> ObtenerPorId(int id)
        {
            var venta = await _repository.ObtenerPorId(id);
            if (venta == null)
            {
                return NotFound();
            }

            var ventaDTO = _mapper.Map<VentaDTO>(venta);
            return Ok(ventaDTO);

        }

        /*
        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("ObtenerConDataRelacionada/{id}")]
        public async Task<ActionResult<CategoriaConProductosDTO>> ObtenerConDataRelacionada(int id)
        {
            var categoria = await _ventaRepository.ObtenerPorIdConData(id);
            if (categoria == null)
            {
                return NotFound();
            }
            var categoriaConProductosDTO = _mapper.Map<CategoriaConProductosDTO>(categoria);
            return Ok(categoriaConProductosDTO);
        }


        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<CategoriaConProductosDTO>>> TodosConDataRelacionada()
        {
            var categorias = await _ventaRepository.ObtenerTodosConData();
            var categoriasDTO = _mapper.Map<IEnumerable<CategoriaConProductosDTO>>(categorias);
            return Ok(categoriasDTO);
        }
        */

        [Authorize(Roles = "Admin, Cliente")]
        [HttpPost]
        public async Task<ActionResult> Insertar(VentaCreacionDTO ventaCreacionDTO)
        {
            var venta = _mapper.Map<Venta>(ventaCreacionDTO);

            var resultado = await _repository.Insertar(venta);
            if (!resultado)
            {
                return NotFound();
            }
            var ventaDTO = _mapper.Map<VentaDTO>(venta);

            return Ok(ventaDTO);
        }

        [Authorize(Roles = "Admin, Cliente")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, VentaCreacionDTO ventaCreacionDTO)
        {
            var venta = await _repository.ObtenerPorId(id);
            if (venta == null)
            {
                return NotFound();
            }
            _mapper.Map(ventaCreacionDTO, venta);
            var resultado = await _repository.Actualizar(venta);
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
