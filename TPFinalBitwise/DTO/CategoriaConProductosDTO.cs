using TPFinalBitwise.Models;

namespace TPFinalBitwise.DTO
{
    public class CategoriaConProductosDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public HashSet<ProductoRespuestaDTO> Productos { get; set; } = new HashSet<ProductoRespuestaDTO>();
    }
}
