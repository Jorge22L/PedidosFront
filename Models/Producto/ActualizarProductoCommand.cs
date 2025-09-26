namespace PedidosFront.Models.Producto
{
    public class ActualizarProductoCommand
    {
        public int ProductoId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencias { get; set; }
        public bool TieneIva { get; set; }
    }
}
