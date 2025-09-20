﻿namespace PedidosFront.Models.Pedido
{
    public class DetallePedidoCommand
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Descuento { get; set; }
        public bool TieneIVA { get; set; }
    }
}
