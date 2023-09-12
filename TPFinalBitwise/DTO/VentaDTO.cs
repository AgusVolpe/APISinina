using TPFinalBitwise.Models;

namespace TPFinalBitwise.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; }
        public string? FechaRealizacion { get; set; }
        public UsuarioDatosDTO Usuario { get; set; } = null!;
        public HashSet<ItemDTO> Items { get; set; } = new HashSet<ItemDTO>();
        public float Total { get; set; }
    }
}
