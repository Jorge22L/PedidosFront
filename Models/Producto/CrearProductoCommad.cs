namespace PedidosFront.Models.Producto
{
    public class CrearProductoCommad
    {
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public int Existencias { get; set; }
        public bool? TieneIVA { get; set; }
    }
}
