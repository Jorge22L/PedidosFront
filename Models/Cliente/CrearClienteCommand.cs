namespace PedidosFront.Models.Cliente
{
    public class CrearClienteCommand
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public bool EsConsumidorFinal { get; set; }
    }
}
