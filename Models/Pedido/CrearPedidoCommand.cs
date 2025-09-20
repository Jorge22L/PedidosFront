namespace PedidosFront.Models.Pedido
{
    public class CrearPedidoCommand
    {
        public int ClienteId { get; set; }
        public string FormaPago { get; set; } = "Contado";
        public string Estado { get; set; } = "Pendiente";
        public List<DetallePedidoCommand> Detalles { get; set; } = new();
    }
}
