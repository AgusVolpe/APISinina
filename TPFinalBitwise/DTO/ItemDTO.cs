namespace TPFinalBitwise.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public ProductoRespuestaDTO Producto { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}
