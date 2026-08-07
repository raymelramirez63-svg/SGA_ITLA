using SGA_ITLA.Domain.Entities.Auditoria;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Services
{
    public interface IAuditoriaApiService
    {
        Task<IEnumerable<RegistroAuditoria>> ObtenerHistorialAsync();
    }

    public class AuditoriaApiService : IAuditoriaApiService
    {
        private readonly HttpClient _httpClient;

        public AuditoriaApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SgaApi");
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObtenerHistorialAsync()
        {
            var response = await _httpClient.GetAsync("Auditoria/GetHistorial");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<RegistroAuditoria>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<RegistroAuditoria>();
            }
            return new List<RegistroAuditoria>();
        }
    }
}