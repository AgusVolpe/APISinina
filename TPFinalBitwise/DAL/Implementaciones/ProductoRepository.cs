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
    }
}
