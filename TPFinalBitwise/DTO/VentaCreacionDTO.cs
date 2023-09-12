using TPFinalBitwise.Models;

namespace TPFinalBitwise.DTO
{
    public class VentaCreacionDTO
    {
        public string? FechaRealizacion { get; set; }
        public string? UserId { get; set; }
        public HashSet<ItemCreacionDTO> Items { get; set; } = new HashSet<ItemCreacionDTO>();
    }
}
