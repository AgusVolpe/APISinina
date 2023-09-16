﻿namespace TPFinalBitwise.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public ProductoRespuestaDTO Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public int VentaId { get; set; }
        public float TotalItem { get; set; }
    }
}
