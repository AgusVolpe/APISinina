using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Models;
using Microsoft.EntityFrameworkCore;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosConData()
        {
            var query = await _context.Productos.Include(c => c.Categoria).ToListAsync();
            return query;
        }

        public async Task<Producto> ObtenerPorIdConData(int id)
        {
            var query = await _context.Productos.Include(c => c.Categoria).FirstOrDefaultAsync(p => p.Id == id);
            return query;
        }

        public async Task<bool> SumarStock(int id, int cantidad)
        {
            var productos = await _context.Productos.ToListAsync();
            var producto = productos.Find(p => p.Id == id);
            var resultado = false;
            if (producto == null)
            {
                return resultado;
            }
            producto.CantidadStock += cantidad;
            _context.Productos.Update(producto);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }

        public async Task<bool> ActualizarStock(HashSet<Item> items, string operacion)
        {
            var resultado = false;
            for (int i = 0; i < items.Count(); i++)
            {
                var productos = await _context.Productos.ToListAsync();
                var item = items.ElementAt(i);
                var producto = productos.Find(p => p.Id == item.ProductoId);
                if (producto == null)
                {
                    return resultado;
                }
                if(operacion == "restar")
                {
                    producto.CantidadStock -= item.Cantidad;
                }else if(operacion == "sumar")
                {
                    producto.CantidadStock += item.Cantidad;
                }
                _context.Productos.Update(producto);
            }
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }
    }
}
