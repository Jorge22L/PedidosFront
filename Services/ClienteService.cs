using Microsoft.JSInterop;
using PedidosFront.Models.Cliente;
using System.Net.Http.Json;

namespace PedidosFront.Services
{
    public class ClienteService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public ClienteService(HttpClient http, IJSRuntime js)
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

        // GET ALL
        public async Task<List<ClienteDto>> GetAllAsync()
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<List<ClienteDto>>("api/Clientes") ?? new();
        }

        // GET BY ID
        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<ClienteDto>($"api/Clientes/{id}");
        }

        // POST (Crear)
        public async Task<bool> CreateAsync(CrearClienteCommand command)
        {
            await AddJwtAsync();
            var response = await _http.PostAsJsonAsync("api/Clientes", command);
            return response.IsSuccessStatusCode;
        }

        // PUT (Actualizar)
        public async Task<bool> UpdateAsync(ActualizarClienteCommand command)
        {
            await AddJwtAsync();
            var response = await _http.PutAsJsonAsync($"api/Clientes/{command.ClienteID}", command);
            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            await AddJwtAsync();
            var response = await _http.DeleteAsync($"api/Clientes/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
