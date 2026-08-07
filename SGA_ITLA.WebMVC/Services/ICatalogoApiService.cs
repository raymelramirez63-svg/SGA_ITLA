using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task<bool> RegistrarAutobusAsync(CreateAutobusDto dto);
        Task<bool> ActualizarAutobusAsync(Autobus autobus);
        Task<bool> EliminarAutobusAsync(int id);
    }
}