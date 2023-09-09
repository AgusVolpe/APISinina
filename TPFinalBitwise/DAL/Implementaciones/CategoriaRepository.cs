using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DAL.Interfaces;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class CategoriaRepository : GenericRepository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ObtenerTodosConData()
        {
            var query = await _context.Categorias.Include(p => p.Productos).ToListAsync();
            return query;
        }

        public async Task<Categoria> ObtenerPorIdConData(int id)
        {
            var query = await _context.Categorias.Include(p => p.Productos).FirstOrDefaultAsync(p => p.Id == id);
            return query;
        }
    }
}