using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.DTO;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IGenericRepository<Item> _repository;
        private readonly IMapper _mapper;
        private readonly IItemRepository _itemRepository;
        private readonly IProductoRepository _productoRepository;

        public ItemController(IGenericRepository<Item> repository, IMapper mapper, IItemRepository itemRepository,
            IProductoRepository productoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _itemRepository = itemRepository;
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> ObtenerTodos()
        {
            var items = await _repository.ObtenerTodos();
            var itemsDTO = _mapper.Map<IEnumerable<ItemDTO>>(items);
            return Ok(itemsDTO);
        }

        [HttpGet("{id}", Name = "GetItem")]
        public async Task<ActionResult<ItemDTO>> ObtenerPorId(int id)
        {
            var item = await _repository.ObtenerPorId(id);
            if (item == null)
            {
                return NotFound();
            }

            var itemDTO = _mapper.Map<ItemDTO>(item);
            return Ok(itemDTO);

        }

        //[ResponseCache(CacheProfileName = "PorDefecto")]
        [HttpGet("ObtenerConDataRelacionada/{id}")]
        public async Task<ActionResult<ItemDTO>> ObtenerConDataRelacionada(int id)
        {
            var item = await _itemRepository.ObtenerPorIdConData(id);
            if (item == null)
            {
                return NotFound();
            }
            var itemDTO = _mapper.Map<ItemDTO>(item);
            return Ok(itemDTO);
        }


        //[ResponseCache(CacheProfileName = "PorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> TodosConDataRelacionada()
        {
            var items = await _itemRepository.ObtenerTodosConData();
            var itemsDTO = _mapper.Map<IEnumerable<ItemDTO>>(items);
            return Ok(itemsDTO);
        }
        

        //[Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Insertar(ItemCreacionDTO itemCreacionDTO)
        {
            var item = _mapper.Map<Item>(itemCreacionDTO);
            var producto = await _productoRepository.ObtenerPorId(item.ProductoId);
            if(producto.CantidadStock < item.Cantidad)
            {
                //return Forbid();
                return NotFound("Producto con stock insuficiente");
            }
            else
            {
                //Insersion del Item
                var resultado = await _repository.Insertar(item);
                if (!resultado)
                {
                    return NotFound();
                }
                var itemDTO = _mapper.Map<ItemDTO>(item);
                
                //Actualizacion de la cantidad de stock
                producto.CantidadStock = producto.CantidadStock - item.Cantidad;
                await _productoRepository.Actualizar(producto);
                
                return Ok(itemDTO);
            }
        }

        //[Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, ItemCreacionDTO itemCreacionDTO)
        {
            var item = await _repository.ObtenerPorId(id);
            if (item == null)
            {
                return NotFound();
            }
            _mapper.Map(itemCreacionDTO, item);
            var resultado = await _repository.Actualizar(item);
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
            var item = await _itemRepository.ObtenerPorId(id);
            var resultado = await _repository.Eliminar(id);
            if (!resultado)
            {
                return BadRequest();
            }
            
            var producto = await _productoRepository.ObtenerPorId(item.ProductoId);
            //Actualizacion de la cantidad de stock
            producto.CantidadStock = producto.CantidadStock + item.Cantidad;
            await _productoRepository.Actualizar(producto);

            return NoContent();
        }
    }
}
