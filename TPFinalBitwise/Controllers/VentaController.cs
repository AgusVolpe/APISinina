using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
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
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IProductoRepository _productoRepository;

        public VentaController(IGenericRepository<Venta> repository, IMapper mapper, IVentaRepository ventaRepository, 
                                IUsuarioRepository usuarioRepository, IItemRepository itemRepository, IProductoRepository productoRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _ventaRepository = ventaRepository;
            _usuarioRepository = usuarioRepository;
            _itemRepository = itemRepository;
            _productoRepository = productoRepository;
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

        
        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("ObtenerConDataRelacionada/{id}")]
        public async Task<ActionResult<VentaDTO>> ObtenerConDataRelacionada(int id)
        {
            var venta = await _ventaRepository.ObtenerPorIdConData(id);
            if (venta == null)
            {
                return NotFound();
            }
            var items = await _itemRepository.ObtenerRelacionVentaItemConData(venta.Id);
            var itemsDTO = _mapper.Map<HashSet<ItemDatosVentaDTO>>(items);
            var ventaDTO = _mapper.Map<VentaDTO>(venta);
            return Ok(ventaDTO);
        }
        

        [ResponseCache(CacheProfileName = "CachePorDefecto")]
        [HttpGet("TodosConDataRelacionada")]
        public async Task<ActionResult<IEnumerable<VentaDTO>>> TodosConDataRelacionada()
        {
            var ventas = await _ventaRepository.ObtenerTodosConData();
            var items = await _itemRepository.ObtenerTodosConData();
            var itemsDTO = _mapper.Map<HashSet<ItemDatosVentaDTO>>(items);
            var ventasDTO = _mapper.Map<IEnumerable<VentaDTO>>(ventas);
            return Ok(ventasDTO);
        }
        

        [Authorize(Roles = "Admin, Registrado")]
        [HttpPost]
        public async Task<ActionResult> Insertar([FromBody] VentaCreacionDTO ventaCreacionDTO)
        {
            //Obtencion y calculo del total de la venta a partir del total por Item.
            var items = ventaCreacionDTO.Items;
            if (items.IsNullOrEmpty())
            {
                return NotFound("No se puede realizar una compra que no contenga productos");
            }
            float TotalVenta = 0;
            var itemsAux = new HashSet<Item>();
            for (int i = 0; i < items.Count(); i++)
            {
                var itemCreacionDTO = items.ElementAt(i);
                var producto = await _productoRepository.ObtenerPorIdConData(itemCreacionDTO.ProductoId);
                var item = _mapper.Map<Item>(itemCreacionDTO);
                item.Producto = producto;
                item.TotalItem = item.Cantidad * producto.Precio;
                TotalVenta += item.TotalItem;
                itemsAux.Add(item);
            }
            var venta = _mapper.Map<Venta>(ventaCreacionDTO);

            //En estas lineas se cargan a la venta los datos del usuario al que se le esta realizando la venta, el total de la venta
            //obtenido como la suma de los totales de cada Item, y se realiza la carga de la lista de Items para que luego de la 
            //insercion de la venta se pueda obtener una respuesta completa con todos los datos necesarios.
            var userId = venta.UserId;
            venta.User = await _usuarioRepository.ObtenerUsuarioPorId(userId);
            venta.Total = TotalVenta;
            venta.Items = itemsAux;

            //Insercion del objeto venta a la Base de Datos
            var resultado = await _repository.Insertar(venta);
            if (!resultado)
            {
                return NotFound();
            }

            //Actualizacion del stock de producto en base a la "salida" de lo que se esta vendiendo.
            await _productoRepository.ActualizarStock(itemsAux, "restar");
            var ventaDTO = _mapper.Map<VentaDTO>(venta);
            return Ok(ventaDTO);
        }


        [Authorize(Roles = "Admin, Registrado")]
        [HttpDelete("CancelarVentaRealizada/{id}")]
        public async Task<ActionResult> Eliminar([FromRoute] int id)
        {
            var venta = await _ventaRepository.ObtenerPorIdConData(id);
            var items = venta.Items;
            var resultado = await _repository.Eliminar(id);
            if (!resultado)
            {
                return BadRequest();
            }

            //Actualizacion del stock de producto en base a la "entrada" por la venta cancelada.
            await _productoRepository.ActualizarStock(items, "sumar");
            
            return NoContent();
        }
    }
}
