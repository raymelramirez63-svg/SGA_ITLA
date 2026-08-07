using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Services
{
    // --- SERVICIO DE AUTORIZACIONES ---
    public interface IAutorizacionApiService
    {
        Task<IEnumerable<Autorizacion>> ObtenerAutorizacionesAsync();
        Task<bool> EmitirTicketAsync(EmitirTicketDto dto);
        Task<bool> RecargarTarjetaAsync(RecargarTarjetaDto dto);
        Task<bool> ActualizarAutorizacionAsync(Autorizacion autorizacion);
        Task<bool> EliminarAutorizacionAsync(int id);
    }

    public class AutorizacionApiService : IAutorizacionApiService
    {
        private readonly HttpClient _httpClient;
        public AutorizacionApiService(IHttpClientFactory factory) { _httpClient = factory.CreateClient("SgaApi"); }

        public async Task<IEnumerable<Autorizacion>> ObtenerAutorizacionesAsync()
        {
            var response = await _httpClient.GetAsync("Autorizaciones");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<Autorizacion>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<Autorizacion>();
            }
            return new List<Autorizacion>();
        }

        public async Task<bool> EmitirTicketAsync(EmitirTicketDto dto) => (await _httpClient.PostAsJsonAsync("Autorizaciones/EmitirTicket", dto)).IsSuccessStatusCode;
        public async Task<bool> RecargarTarjetaAsync(RecargarTarjetaDto dto) => (await _httpClient.PostAsJsonAsync("Autorizaciones/RecargarTarjeta", dto)).IsSuccessStatusCode;
        public async Task<bool> ActualizarAutorizacionAsync(Autorizacion autorizacion) => (await _httpClient.PutAsJsonAsync("Autorizaciones", autorizacion)).IsSuccessStatusCode;
        public async Task<bool> EliminarAutorizacionAsync(int id) => (await _httpClient.DeleteAsync($"Autorizaciones/{id}")).IsSuccessStatusCode;
    }

    // --- SERVICIO DE SOLICITUDES ---
    public interface ISolicitudApiService
    {
        Task<IEnumerable<SolicitudAutorizacion>> ObtenerMisSolicitudesAsync();
        Task<IEnumerable<SolicitudAutorizacion>> ObtenerPendientesAsync();
        Task<bool> CrearSolicitudAsync(SGA_ITLA.Domain.Enums.TipoAutorizacion tipo, string? comentario);
        Task<bool> AprobarSolicitudAsync(int id, int pagoId, decimal? monto);
        Task<bool> RechazarSolicitudAsync(int id, string motivo);
    }

    public class SolicitudApiService : ISolicitudApiService
    {
        private readonly HttpClient _httpClient;
        public SolicitudApiService(IHttpClientFactory factory) { _httpClient = factory.CreateClient("SgaApi"); }

        public async Task<IEnumerable<SolicitudAutorizacion>> ObtenerMisSolicitudesAsync()
        {
            var response = await _httpClient.GetAsync("Solicitud/MisSolicitudes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<SolicitudAutorizacion>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<SolicitudAutorizacion>();
            }
            return new List<SolicitudAutorizacion>();
        }

        public async Task<IEnumerable<SolicitudAutorizacion>> ObtenerPendientesAsync()
        {
            var response = await _httpClient.GetAsync("Solicitud/Pendientes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OperationResultApi<IEnumerable<SolicitudAutorizacion>>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.Data ?? new List<SolicitudAutorizacion>();
            }
            return new List<SolicitudAutorizacion>();
        }

        public async Task<bool> CrearSolicitudAsync(SGA_ITLA.Domain.Enums.TipoAutorizacion tipo, string? comentario) =>
            (await _httpClient.PostAsJsonAsync("Solicitud/Crear", new { TipoSolicitado = tipo, Comentario = comentario })).IsSuccessStatusCode;

        public async Task<bool> AprobarSolicitudAsync(int id, int pagoId, decimal? monto) =>
            (await _httpClient.PostAsJsonAsync($"Solicitud/Aprobar/{id}", new { PagoId = pagoId, Monto = monto })).IsSuccessStatusCode;

        public async Task<bool> RechazarSolicitudAsync(int id, string motivo) =>
            (await _httpClient.PostAsJsonAsync($"Solicitud/Rechazar/{id}", new { Motivo = motivo })).IsSuccessStatusCode;
    }
}