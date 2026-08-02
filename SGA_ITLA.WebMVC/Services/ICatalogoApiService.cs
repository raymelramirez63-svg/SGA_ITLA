using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios; 

namespace SGA_ITLA.WebMVC.Services
{
    public interface ICatalogoApiService
    {
        Task<IEnumerable<Ruta>> ObtenerRutasAsync();
        Task<Ruta?> ObtenerRutaPorIdAsync(int id);
        Task<bool> RegistrarRutaAsync(CreateRutaDto dto);
        Task<bool> ActualizarRutaAsync(Ruta ruta);
        Task<bool> EliminarRutaAsync(int id);

        Task<IEnumerable<Autobus>> ObtenerAutobusesAsync();
        Task<IEnumerable<Usuario>> ObtenerConductoresAsync();
    }
}