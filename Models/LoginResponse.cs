namespace PedidosFront.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira {  get; set; }
    }
}
