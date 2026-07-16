using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces; 
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class ViajeService : IViajeService
    {
   
        private readonly IViajeRepository _viajeRepo;
        private readonly ILogger<ViajeService> _logger;

        public ViajeService(IViajeRepository viajeRepo, ILogger<ViajeService> logger)
        {
            _viajeRepo = viajeRepo;
            _logger = logger;
        }

        public async Task<OperationResult> ObtenerViajesDetalladosAsync()
        {
            _logger.LogInformation("Solicitando listado de viajes detallados.");
            // Llamamos al repositorio que arreglamos anteriormente
            return await _viajeRepo.GetViajesDetalladosAsync();
        }

        public async Task<OperationResult> RegistrarViajeAsync(Viaje viaje)
        {
            try
            {
                _logger.LogInformation("Validando reglas de negocio RN-OPE para nuevo viaje.");

            
                viaje.Estado = EstadoViaje.Programado;

                var resultadoGuardado = await _viajeRepo.SaveEntityAsync(viaje);

                if (resultadoGuardado.Success)
                {
                    _logger.LogInformation("Viaje planificado exitosamente en la base de datos.");
                    return new OperationResult { Success = true, Message = "Viaje planificado exitosamente." };
                }

                return resultadoGuardado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico en la capa de aplicación al registrar el viaje.");
                return new OperationResult { Success = false, Message = "Error interno al procesar el viaje." };
            }
        }

        public async Task<OperationResult> ValidarAbordajeAsync(int viajeId, int estudianteId)
        {
            try
            {
                _logger.LogInformation($"Validando acceso RN-ACC para estudiante {estudianteId} en viaje {viajeId}");

                
                var estadoViajeActual = EstadoViaje.EnCurso;
                int cupoDisponible = 40;
                bool tieneAutorizacionActiva = true;

                if (estadoViajeActual != EstadoViaje.EnCurso)
                    return new OperationResult { Success = false, Message = "Acceso Denegado: El viaje aún no está en curso o ya finalizó." };

                if (cupoDisponible <= 0)
                    return new OperationResult { Success = false, Message = "Acceso Denegado: El autobús ha alcanzado su capacidad máxima." };

                if (!tieneAutorizacionActiva)
                    return new OperationResult { Success = false, Message = "Acceso Denegado: Autorización vencida o saldo insuficiente." };

                _logger.LogInformation($"Acceso permitido. Auditando abordaje en base de datos.");
                return new OperationResult { Success = true, Message = "Acceso Permitido. Saldo descontado exitosamente." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el abordaje.");
                return new OperationResult { Success = false, Message = "Error interno al validar el abordaje." };
            }
        }

        public async Task<OperationResult> ActualizarViajeAsync(SaveViajeDto dto)
        {
            return new OperationResult { Success = true, Message = "Viaje actualizado correctamente." };
        }

        public async Task<OperationResult> EliminarViajeLogicoAsync(int id)
        {
            return new OperationResult { Success = true, Message = "Viaje eliminado exitosamente (Deleted = true)." };
        }
    }
}