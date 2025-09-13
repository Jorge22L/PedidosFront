using Microsoft.JSInterop;
using PedidosFront.Models;
using System.Net.Http.Json;

namespace PedidosFront.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        private const string TOKEN_KEY = "EstaEsUnaClaveMuyLargaParaQueFuncioneElJwt";

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // Login: Enviar email y password a la API
        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Usuarios/Login", request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if(loginResponse is not null && !string.IsNullOrEmpty(loginResponse.Token))
            {
                // Guardar Token en Local Storage
                await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, loginResponse.Token);

                // Inyectar en HttpClient para próximas llamads
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);

                return true;
            }

            return false;
        }

        // Logout: quitar el token
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            _http.DefaultRequestHeaders.Authorization = null;
        }

        // Verifica si el token sigue guardado

        public async Task<string> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);
        }
    }
}
