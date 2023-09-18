using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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


        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [Authorize(Roles = "Admin, Registrado")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> ObtenerTodos()
        {
            var items = await _repository.ObtenerTodos();
            var itemsDTO = _mapper.Map<IEnumerable<ItemDTO>>(items);
            return Ok(itemsDTO);
        }


        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [Authorize(Roles = "Admin, Registrado")]
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


        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [Authorize(Roles = "Admin, Registrado")]
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


        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [Authorize(Roles = "Admin, Registrado")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> TodosConDataRelacionada()
        {
            var items = await _itemRepository.ObtenerTodosConData();
            var itemsDTO = _mapper.Map<IEnumerable<ItemDTO>>(items);
            return Ok(itemsDTO);
        }


        [Authorize(Roles = "Admin, Registrado")]
        [HttpPost]
        public async Task<ActionResult> Insertar([FromBody] ItemCreacionDTO itemCreacionDTO)
        {
            var item = _mapper.Map<Item>(itemCreacionDTO);
            var producto = await _productoRepository.ObtenerPorId(item.ProductoId);
            if(producto.CantidadStock < item.Cantidad)
            {
                return NotFound("Producto con stock insuficiente");
            }
            else
            {
                item.TotalItem = producto.Precio * item.Cantidad;
                
                //Insersion del Item
                var resultado = await _repository.Insertar(item);
                if (!resultado)
                {
                    return NotFound();
                }
                var itemDTO = _mapper.Map<ItemDTO>(item);
                
                //Actualizacion de la cantidad de stock
                HashSet<Item> items = new HashSet<Item>();
                items.Add(item);
                await _productoRepository.ActualizarStock(items,"restar");

                //return CreatedAtAction("ItemCreado", itemDTO);
                return Ok(itemDTO);
            }
        }


        [Authorize(Roles = "Admin, Registrado")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar([FromRoute] int id, [FromBody] ItemCreacionDTO itemCreacionDTO)
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


        [Authorize(Roles = "Admin, Registrado")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar([FromBody] int id)
        {
            var item = await _itemRepository.ObtenerPorId(id);
            HashSet<Item> items = new HashSet<Item>();
            items.Add(item);

            var resultado = await _repository.Eliminar(id);
            if (!resultado)
            {
                return BadRequest();
            }

            //Actualizacion de la cantidad de stock
            await _productoRepository.ActualizarStock(items, "sumar");

            return NoContent();
        }
    }
}
