using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> ObtenerTodosConData()
        {
            var query = await _context.Items.Include(p => p.Producto).ToListAsync();
            return query;
        }

        public async Task<Item> ObtenerPorIdConData(int id)
        {
            var query = await _context.Items.Include(p => p.Producto).FirstOrDefaultAsync(p => p.Id == id);
            return query;
        }
    }
}
