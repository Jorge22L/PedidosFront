using Microsoft.JSInterop;
using PedidosFront.Models.Login;
using System.Net.Http.Json;

namespace PedidosFront.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        // 🔑 Aquí solo usamos una clave para almacenar el token en el browser
        private const string TOKEN_KEY = "jwt_token";

        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        // Login
        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Usuarios/Login", request);

            if (!response.IsSuccessStatusCode)
                return false;

            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (loginResponse is not null && !string.IsNullOrEmpty(loginResponse.Token))
            {
                // Guardar Token en Local Storage bajo "jwt_token"
                await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, loginResponse.Token);

                // Configurar HttpClient
                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);

                return true;
            }

            return false;
        }

        // Logout
        public async Task LogoutAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            _http.DefaultRequestHeaders.Authorization = null;
        }

        // Obtener token actual
        public async Task<string?> GetTokenAsync()
        {
            return await _js.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);
        }
    }
}