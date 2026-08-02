using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.WebMVC.Services
{
    public interface ITransporteApiService
    {
        // Métodos para Horarios
        Task<IEnumerable<Horario>> ObtenerHorariosAsync();
        Task<Horario?> ObtenerHorarioPorIdAsync(int id);
        Task<bool> RegistrarHorarioAsync(object dto); 
        Task<bool> ActualizarHorarioAsync(Horario horario);
        Task<bool> EliminarHorarioAsync(int id);

        // Métodos para Viajes
        Task<IEnumerable<Viaje>> ObtenerViajesAsync();
        Task<Viaje?> ObtenerViajePorIdAsync(int id);
        Task<bool> RegistrarViajeAsync(object dto);
        Task<bool> ActualizarViajeAsync(Viaje viaje);
        Task<bool> EliminarViajeAsync(int id);
    }
}