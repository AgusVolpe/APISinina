using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        public Task<IEnumerable<Producto>> ObtenerTodosConData();
        public Task<Producto> ObtenerPorIdConData(int id);
    }
}
