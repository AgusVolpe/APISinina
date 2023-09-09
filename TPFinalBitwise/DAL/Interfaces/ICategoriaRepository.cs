using TPFinalBitwise.Models;

namespace TPFinalBitwise.DAL.Interfaces
{
    public interface ICategoriaRepository : IGenericRepository<Categoria>
    {
        public Task<IEnumerable<Categoria>> ObtenerTodosConData();
        public Task<Categoria> ObtenerPorIdConData(int id);
    }
}
