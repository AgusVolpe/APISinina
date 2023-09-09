namespace TPFinalBitwise.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int CodigoBarras { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set;}
        public int CantidadStock { get; set; }
        public float Precio { get; set;}
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;
    }
}
