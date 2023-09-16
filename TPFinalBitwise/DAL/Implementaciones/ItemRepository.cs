using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        
        public async Task<IEnumerable<Item>> ObtenerRelacionVentaItemConData(int ventaId)
        {
            var query = await _context.Items.Include(p => p.Producto).ToListAsync();
            var items = query.FindAll(i => i.VentaId == ventaId);
            return items;
        }

        /*public async Task<bool> ActualizarTotalItem(int id, float totalItem)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            var resultado = false;
            if (item == null)
            {
                return resultado;
            }
            item.TotalItem = totalItem;
            _context.Items.Update(item);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }*/
    }
}
