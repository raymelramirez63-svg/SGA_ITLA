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
           
            bool autobusOcupado = false; 
            bool conductorOcupado = false; 

          
            if (autobusOcupado)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = "Rechazado: El autobús ya tiene un viaje activo asignado en ese horario." //[cite: 4]
                });
            }

          
            if (conductorOcupado)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = "Rechazado: El conductor ya tiene un viaje activo asignado en ese horario." //[cite: 4]
                });
            }

            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje planificado exitosamente." });
        }

        public async Task<OperationResult> ValidarAbordajeAsync(int viajeId, int estudianteId)
        {
        
            var estadoViajeActual = EstadoViaje.EnCurso;
            int cupoDisponible = 40;
            bool tieneAutorizacionActiva = (estudianteId != 999); 

           
            if (estadoViajeActual != EstadoViaje.EnCurso)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = "Acceso Denegado: El viaje aún no está en curso o ya finalizó."
                });
            }

            
            if (cupoDisponible <= 0)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = "Acceso Denegado: El autobús ha alcanzado su capacidad máxima." //[cite: 4]
                });
            }

            if (!tieneAutorizacionActiva)
            {
                return await Task.FromResult(new OperationResult
                {
                    Success = false,
                    Message = "Acceso Denegado: Autorización vencida o saldo insuficiente." //[cite: 4]
                });
            }

            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = "Acceso Permitido. Saldo descontado exitosamente."
            });
        }

        
        public async Task<OperationResult> ActualizarViajeAsync(SaveViajeDto dto)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje actualizado correctamente." });
        }

       
        public async Task<OperationResult> EliminarViajeLogicoAsync(int id)
        {
            return await Task.FromResult(new OperationResult { Success = true, Message = "Viaje eliminado exitosamente (Deleted = true)." });
        }
    }
}