namespace TPFinalBitwise.DTO
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public int CodigoBarras { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CantidadStock { get; set; }
        public float Precio { get; set; }
        public string? NombreCategoria { get; set; }
    }
}
