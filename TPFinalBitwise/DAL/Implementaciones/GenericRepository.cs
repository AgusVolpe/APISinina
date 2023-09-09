using Microsoft.EntityFrameworkCore;
using TPFinalBitwise.DAL.DataContext;
using TPFinalBitwise.DAL.Interfaces;

namespace TPFinalBitwise.DAL.Implementaciones
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Actualizar(T entidad)
        {
            bool resultado = false;

            _context.Set<T>().Update(entidad);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool resultado = false;
            T entidad = await ObtenerPorId(id);
            if (entidad != null)
            {
                _context.Set<T>().Remove(entidad);
                resultado = await _context.SaveChangesAsync() > 0;
            }
            return resultado;
        }

        public async Task<bool> Insertar(T entidad)
        {
            bool resultado = false;

            await _context.Set<T>().AddAsync(entidad);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }

        public async Task<T> ObtenerPorId(int id)
        {
            T entidad = await _context.Set<T>().FindAsync(id);
            return entidad;
        }

        public async Task<IEnumerable<T>> ObtenerTodos()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
