using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly ApplicationDbContext _context;

        public VentaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> ObtenerTodosConData()
        {
            var query = await _context.Ventas
                                .Include(u => u.User)
                                .Include(i => i.Items).ToListAsync();
            return query;
        }

        public async Task<Venta> ObtenerPorIdConData(int id)
        {
            var query = await _context.Ventas.Include(v => v.Items).Include(v => v.User).FirstOrDefaultAsync(v => v.Id == id);
            return query;
        }
    }
}
