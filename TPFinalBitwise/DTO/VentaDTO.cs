using TPFinalBitwise.Models;

namespace TPFinalBitwise.DTO
{
    public class VentaDTO
    {
        public int Id { get; set; }
        public string? FechaRealizacion { get; set; }
        public UsuarioDatosVentaDTO User { get; set; } = null!;
        public HashSet<ItemDatosVentaDTO> Items { get; set; } = new HashSet<ItemDatosVentaDTO>();
        public float Total { get; set; }
    }
}
