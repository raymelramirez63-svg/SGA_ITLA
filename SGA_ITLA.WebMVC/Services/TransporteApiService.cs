using SGA_ITLA.Domain.Entities.Transporte;
using System.Text.Json;

namespace SGA_ITLA.WebMVC.Services
{
    public class TransporteApiService : ITransporteApiService
    {
        private readonly HttpClient _httpClient;

        public TransporteApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SgaApi");
        }

        // --- HORARIOS ---
        public async Task<IEnumerable<Horario>> ObtenerHorariosAsync()
        {
            var response = await _httpClient.GetAsync("Transporte/horarios");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Horario>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Horario>();
            }
            return new List<Horario>();
        }

        public async Task<Horario?> ObtenerHorarioPorIdAsync(int id)
        {
            var horarios = await ObtenerHorariosAsync();
            return horarios.FirstOrDefault(h => h.Id == id);
        }

        public async Task<bool> RegistrarHorarioAsync(object dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Transporte/horario", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarHorarioAsync(Horario horario)
        {
            var response = await _httpClient.PutAsJsonAsync("Transporte/horario", horario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarHorarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Transporte/horario/{id}");
            return response.IsSuccessStatusCode;
        }

        // --- VIAJES ---
        public async Task<IEnumerable<Viaje>> ObtenerViajesAsync()
        {
            var response = await _httpClient.GetAsync("Transporte/viajes");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Viaje>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Viaje>();
            }
            return new List<Viaje>();
        }

        public async Task<Viaje?> ObtenerViajePorIdAsync(int id)
        {
            var viajes = await ObtenerViajesAsync();
            return viajes.FirstOrDefault(v => v.Id == id);
        }

        public async Task<bool> RegistrarViajeAsync(object dto)
        {
            var response = await _httpClient.PostAsJsonAsync("Transporte/viaje", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarViajeAsync(Viaje viaje)
        {
            var response = await _httpClient.PutAsJsonAsync("Transporte/viaje", viaje);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarViajeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Transporte/viaje/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}