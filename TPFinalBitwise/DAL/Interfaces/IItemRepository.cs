using Microsoft.AspNetCore.Mvc;
using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        public Task<IEnumerable<Item>> ObtenerTodosConData();
        public Task<Item> ObtenerPorIdConData(int id);
        public Task<IEnumerable<Item>> ObtenerRelacionVentaItemConData(int ventaId);
        //public Task<bool> ActualizarTotalItem(int id, float totalItem);
    }
}
