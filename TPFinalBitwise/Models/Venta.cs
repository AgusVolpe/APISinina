namespace TPFinalBitwise.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public float Total { get; set; }
        public string? UserId { get; set; }
        public Usuario User { get; set; } = null!;
        public HashSet<Item> Items { get; set; } = new HashSet<Item>();
    }

}
