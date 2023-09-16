using TPFinalBitwise.Models;

namespace TPFinalBitwise.DTO
{
    public class ItemDatosVentaDTO
    {
        public int Id { get; set; }
        public ProductoRespuestaDTO Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public float TotalItem { get; set; }
    }
}
