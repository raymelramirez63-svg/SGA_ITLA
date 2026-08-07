using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Services
{
    public class CatalogoApiService : ICatalogoApiService
    {
        private readonly HttpClient _httpClient;

        public CatalogoApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SgaApi");
        }

        public async Task<IEnumerable<Ruta>> ObtenerRutasAsync()
        {
            var response = await _httpClient.GetAsync("Catalogo/rutas");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Ruta>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Ruta>();
            }
            return new List<Ruta>();
        }

        public async Task<Ruta?> ObtenerRutaPorIdAsync(int id)
        {
            var rutas = await ObtenerRutasAsync();
            return rutas.FirstOrDefault(r => r.Id == id);
        }

        public async Task<bool> RegistrarRutaAsync(CreateRutaDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Catalogo/ruta", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarRutaAsync(Ruta ruta)
        {
            var response = await _httpClient.PutAsJsonAsync("Catalogo/ruta", ruta);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> EliminarRutaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Catalogo/ruta/{id}");
            if (response.IsSuccessStatusCode) return "OK";

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OperationResultApi<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Message ?? "Error al intentar desactivar la ruta.";
        }

        public async Task<IEnumerable<Autobus>> ObtenerAutobusesAsync()
        {
            var response = await _httpClient.GetAsync("Catalogo/autobuses");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Autobus>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Autobus>();
            }
            return new List<Autobus>();
        }

        public async Task<IEnumerable<Usuario>> ObtenerConductoresAsync()
        {
            var response = await _httpClient.GetAsync("Catalogo/conductores");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Usuario>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Usuario>();
            }
            return new List<Usuario>();
        }

        public async Task<bool> RegistrarAutobusAsync(CreateAutobusDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Catalogo/autobus", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarAutobusAsync(Autobus autobus)
        {
            var response = await _httpClient.PutAsJsonAsync("Catalogo/autobus", autobus);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> EliminarAutobusAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Catalogo/autobus/{id}");
            if (response.IsSuccessStatusCode) return "OK";

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OperationResultApi<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Message ?? "Error al intentar desactivar la unidad.";
        }
    }

    public class OperationResultApi<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
