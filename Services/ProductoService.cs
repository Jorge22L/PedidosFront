using Microsoft.JSInterop;
using PedidosFront.Models.Producto;
using System.Net.Http.Json;

namespace PedidosFront.Services
{
    public class ProductoService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public ProductoService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task AddJwtAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "jwt_token");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }


        }

        // GET 
        public async Task<List<ProductoDto?>> GetAllAsync()
        {
            await AddJwtAsync();

            return await _http.GetFromJsonAsync<List<ProductoDto>>("api/Productos");
        }
    }
}
