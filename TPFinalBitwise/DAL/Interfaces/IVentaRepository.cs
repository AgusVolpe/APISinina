using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface IVentaRepository : IGenericRepository<Venta>
    {
        public Task<IEnumerable<Venta>> ObtenerTodosConData();
        public Task<Venta> ObtenerPorIdConData(int id);
    }
}
