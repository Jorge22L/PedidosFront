using Microsoft.JSInterop;
using PedidosFront.Models.Pedido;
using System.Net.Http.Json;

namespace PedidosFront.Services
{
    public class PedidoService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public PedidoService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddJwtAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "jwt_token");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        // GET ALL
        public async Task<List<PedidoDto>> GetAllAsync()
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<List<PedidoDto>>("api/Pedidos") ?? new();
        }

        // GET BY ID
        public async Task<PedidoDto?> GetByIdAsync(int id)
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<PedidoDto>($"api/Pedidos/{id}");
        }

        // CREATE Pedido (Encabezado + Detalles)
        public async Task<bool> CreateAsync(CrearPedidoCommand command)
        {
            await AddJwtAsync();
            var response = await _http.PostAsJsonAsync("api/Pedidos", command);
            return response.IsSuccessStatusCode;
        }
    }
}
