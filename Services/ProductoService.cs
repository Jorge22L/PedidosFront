using Microsoft.JSInterop;
using PedidosFront.Models.Producto;
using PedidosFrontend.Models.Commons;
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
        public async Task<PagedResponse<ProductoDto>> ObtenerProductosAsync(int pageNumber, int pageSize)
        {
            await AddJwtAsync();
            var resp =
                await _http.
                GetAsync($"api/Productos?pageNumber={pageNumber}&pageSize={pageSize}");

            if (!resp.IsSuccessStatusCode)
            {
                return new PagedResponse<ProductoDto>();
            }
            // Lee body
            var items = await resp.Content.ReadFromJsonAsync<List<ProductoDto>>();

            // Leer headers
            var paginacion = new PagedResponse<ProductoDto>
            {
                Items = items ?? new List<ProductoDto>(),
                PageNumber = pageNumber,
                PageSize = pageSize,

            };

            if (resp.Headers.TryGetValues("X-Page", out var pageVal) &&
                int.TryParse(pageVal.FirstOrDefault(), out var currentPage))
            {
                paginacion.PageNumber = currentPage;
            }
            if (resp.Headers.TryGetValues("X-Page-Size", out var sizeVal) &&
                int.TryParse(sizeVal.FirstOrDefault(), out var currentSize))
            {
                paginacion.PageSize = currentSize;
            }

            if (resp.Headers.TryGetValues("X-Total-Count", out var totalVal) &&
                int.TryParse(totalVal.FirstOrDefault(), out var total))
            {
                paginacion.TotalCount = total;
            }



            return paginacion;
        }

        public async Task<List<ProductoDto>> ObtenerListadoProductos()
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<List<ProductoDto>>($"api/Productos");
        }

        public async Task<ProductoDto?> ObtenerProductoPorIdAsync(int id)
        {
            await AddJwtAsync();
            return await _http.GetFromJsonAsync<ProductoDto>($"api/Productos/{id}");
        }

        public async Task<bool> CrearProductoAsync(CrearProductoCommad command)
        {
            await AddJwtAsync();
            var response = await _http.PostAsJsonAsync("api/Productos", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualProductoAsync(int id, ActualizarProductoCommand command)
        {
            await AddJwtAsync();
            var response = await _http.PutAsJsonAsync($"api/Productos/{id}", command);
            return response.IsSuccessStatusCode && response.Content != null;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            await AddJwtAsync();
            var response = await _http.DeleteAsync($"api/Productos/{id}");
            return response.IsSuccessStatusCode || response.Content != null;
        }
    }
}
