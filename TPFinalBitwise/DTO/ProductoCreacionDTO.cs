namespace TPFinalBitwise.DTO
{
    public class ProductoCreacionDTO
    {
        public int CodigoBarras { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CantidadStock { get; set; }
        public float Precio { get; set; }
        public int CategoriaId { get; set; }
    }
}
