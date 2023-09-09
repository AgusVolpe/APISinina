namespace TPFinalBitwise.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public HashSet<Producto> Productos { get; set; } = new HashSet<Producto>();
    }
}
