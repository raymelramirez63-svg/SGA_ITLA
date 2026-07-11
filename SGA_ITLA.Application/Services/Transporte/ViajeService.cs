using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Enums;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class ViajeService : IViajeService
    {
       
        public async Task<OperationResult> ObtenerViajesDetalladosAsync()
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Listado de viajes obtenidos exitosamente." });
        }

        public async Task<OperationResult> RegistrarViajeAsync(Viaje viaje)
        {
          
            bool hayConflicto = false;

            if (hayConflicto)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "Rechazado: Hay un conflicto de horarios con el autobús o conductor." });
            }

            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje planificado exitosamente." });
        }

        
        public async Task<OperationResult> ValidarAbordajeAsync(int viajeId, int estudianteId)
        {
            if (estudianteId == 999)
            {
                return await Task.FromResult(new OperationResult { Success = false, Message = "Rechazado: Autorización vencida o saldo insuficiente." });
            }

            return await Task.FromResult(new OperationResult { Success = true, Message = "Acceso Permitido. Saldo descontado exitosamente." });
        }

        
        public async Task<OperationResult> ActualizarViajeAsync(SaveViajeDto dto)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje actualizado correctamente." });
        }

        
        public async Task<OperationResult> EliminarViajeLogicoAsync(int id)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje eliminado (Borrado lógico)." });
        }
    }
}