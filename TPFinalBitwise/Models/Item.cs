namespace TPFinalBitwise.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}
