using System.Text.Json;

namespace SGA_ITLA.WebMVC.Services
{
    public interface IAuthApiService
    {
        Task<LoginResponseApi?> LoginAsync(string email, string password);
    }

    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SgaApiClean");
        }

        public async Task<LoginResponseApi?> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/login", new { Email = email, Password = password });

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoginResponseApi>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }
    }

    public class LoginResponseApi
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
