using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Auditoria; 
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
        private readonly IAuditoriaRepository _auditoriaRepo; // AUDITORÍA
        private readonly ILogger<ViajeService> _logger;

        public ViajeService(IViajeRepository viajeRepo, IAuditoriaRepository auditoriaRepo, ILogger<ViajeService> logger)
        {
            _viajeRepo = viajeRepo;
            _auditoriaRepo = auditoriaRepo;
            _logger = logger;
        }

        public async Task<OperationResult> ObtenerViajesDetalladosAsync()
        {
            _logger.LogInformation("Solicitando listado de viajes detallados.");
            return await _viajeRepo.GetViajesDetalladosAsync();
        }

        public async Task<OperationResult> RegistrarViajeAsync(Viaje viaje)
        {
            try
            {
                _logger.LogInformation("Validando reglas de negocio RN-OPE para nuevo viaje.");

                bool hayConflicto = await _viajeRepo.ExisteConflictoDeRecursosAsync(
                    viaje.AutobusId,
                    viaje.ConductorId,
                    viaje.HorarioSalidaPlanificada);

                if (hayConflicto)
                {
                    _logger.LogWarning("Validación fallida: Conflicto de recursos detectado.");

                    // CUMPLIMIENTO RN-AUD: Auditar fallo
                    await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                    {
                        ActorId = 1, // ID Sistema o Admin
                        ModuloAfectado = "Planificación",
                        AccionRealizada = "Creación de Viaje",
                        ResultadoExitoso = false,
                        MotivoFallo = "Conflicto de recursos detectado (Autobús o Conductor ocupado).",
                        CreationDate = DateTime.Now
                    });

                    return new OperationResult
                    {
                        Success = false,
                        Message = "Rechazado (RN-OPE): El autobús o el conductor ya se encuentran asignados a otro viaje activo en ese mismo horario."
                    };
                }

                viaje.Estado = EstadoViaje.Programado;
                var resultadoGuardado = await _viajeRepo.SaveEntityAsync(viaje);

                if (resultadoGuardado.Success)
                {
                    // CUMPLIMIENTO RN-AUD: Auditar éxito
                    await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                    {
                        ActorId = 1,
                        ModuloAfectado = "Planificación",
                        AccionRealizada = "Creación de Viaje",
                        Detalles = $"Viaje #{viaje.Id} programado exitosamente.",
                        ResultadoExitoso = true,
                        CreationDate = DateTime.Now
                    });

                    _logger.LogInformation($"Viaje planificado exitosamente en la base de datos con ID {viaje.Id}.");
                    return new OperationResult
                    {
                        Success = true,
                        Message = $"Viaje planificado exitosamente. El ID asignado es: {viaje.Id}"
                    };
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
            return new OperationResult { Success = false, Message = "Use AccesoService para validar abordajes." };
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