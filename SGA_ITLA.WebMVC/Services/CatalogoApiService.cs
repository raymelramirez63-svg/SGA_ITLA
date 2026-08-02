using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios; 
using System.Text.Json;

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

        public async Task<bool> EliminarRutaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Catalogo/ruta/{id}");
            return response.IsSuccessStatusCode;
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
    }

    public class OperationResultApi<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}