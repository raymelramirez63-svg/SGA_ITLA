using SGA_ITLA.Domain.Entities.Usuarios;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Services
{
    public interface IUsuarioApiService
    {
        Task<IEnumerable<Usuario>> ObtenerUsuariosAsync();
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
        Task<bool> RegistrarUsuarioAsync(Usuario usuario);
        Task<bool> ActualizarUsuarioAsync(Usuario usuario);
        Task<string> EliminarUsuarioAsync(int id);
    }

    public class UsuarioApiService : IUsuarioApiService
    {
        private readonly HttpClient _httpClient;

        public UsuarioApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SgaApi");
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            var response = await _httpClient.GetAsync("Usuario");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Usuario>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Usuario>();
            }
            return new List<Usuario>();
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Usuario/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<Usuario>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data;
            }
            return null;
        }

        public async Task<bool> RegistrarUsuarioAsync(Usuario usuario)
        {
            var response = await _httpClient.PostAsJsonAsync("Usuario", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarUsuarioAsync(Usuario usuario)
        {
            var response = await _httpClient.PutAsJsonAsync("Usuario", usuario);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> EliminarUsuarioAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Usuario/{id}");
            if (response.IsSuccessStatusCode) return "OK";

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OperationResultApi<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Message ?? "Error al suspender usuario";
        }
    }
}