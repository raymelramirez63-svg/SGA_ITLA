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
        private readonly IAuditoriaRepository _auditoriaRepo;
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
                    await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                    {
                        ActorId = 1,
                        ModuloAfectado = "Planificación",
                        AccionRealizada = "Creación de Viaje",
                        ResultadoExitoso = false,
                        MotivoFallo = "Conflicto de recursos detectado (Autobús o Conductor ocupado).",
                        CreationDate = DateTime.Now
                    });

                    return new OperationResult { Success = false, Message = "Rechazado (RN-OPE): El autobús o el conductor ya se encuentran asignados a otro viaje activo." };
                }

                viaje.Estado = EstadoViaje.Programado;
                var resultadoGuardado = await _viajeRepo.SaveEntityAsync(viaje);

                if (resultadoGuardado.Success)
                {
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
                    return new OperationResult { Success = true, Message = $"Viaje planificado exitosamente. El ID asignado es: {viaje.Id}" };
                }

                return resultadoGuardado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico en la capa de aplicación al registrar el viaje.");
                return new OperationResult { Success = false, Message = "Error interno al procesar el viaje." };
            }
        }

        public async Task<OperationResult> ActualizarViajeAsync(Viaje viaje)
        {
            var originalResult = await _viajeRepo.GetByIdAsync(viaje.Id);
            if (!originalResult.Success || originalResult.Data == null) return new OperationResult { Success = false, Message = "Viaje no encontrado." };

            var viajeOriginal = (Viaje)originalResult.Data;
            viajeOriginal.Estado = viaje.Estado;
            return await _viajeRepo.UpdateEntityAsync(viajeOriginal);
        }

        public async Task<OperationResult> EliminarViajeAsync(int id)
        {
            var originalResult = await _viajeRepo.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return new OperationResult { Success = false, Message = "Viaje no encontrado." };

            var viaje = (Viaje)originalResult.Data;
            if (viaje.Estado == EstadoViaje.EnCurso || viaje.Estado == EstadoViaje.Completado)
            {
                return new OperationResult { Success = false, Message = $"No se puede eliminar un viaje en estado '{viaje.Estado}'." };
            }

            return await _viajeRepo.DeleteEntityAsync(viaje);
        }

        public async Task<OperationResult> CambiarEstadoViajeAsync(int viajeId, int nuevoEstadoId)
        {
            var originalResult = await _viajeRepo.GetByIdAsync(viajeId);
            if (!originalResult.Success || originalResult.Data == null) return new OperationResult { Success = false, Message = "Viaje no encontrado." };

            var viajeOriginal = (Viaje)originalResult.Data;
            var estadoAnterior = viajeOriginal.Estado;
            var nuevoEstado = (EstadoViaje)nuevoEstadoId;

            bool transitionValid = false;
            if (estadoAnterior == EstadoViaje.Programado && (nuevoEstado == EstadoViaje.EnCurso || nuevoEstado == EstadoViaje.Cancelado || nuevoEstado == EstadoViaje.Retrasado)) transitionValid = true;
            else if (estadoAnterior == EstadoViaje.EnCurso && (nuevoEstado == EstadoViaje.Completado || nuevoEstado == EstadoViaje.Retrasado)) transitionValid = true;
            else if (estadoAnterior == EstadoViaje.Retrasado && (nuevoEstado == EstadoViaje.EnCurso || nuevoEstado == EstadoViaje.Cancelado || nuevoEstado == EstadoViaje.Programado)) transitionValid = true;

            if (!transitionValid) return new OperationResult { Success = false, Message = $"Transición de estado inválida de {estadoAnterior} a {nuevoEstado}." };

            viajeOriginal.Estado = nuevoEstado;
            var result = await _viajeRepo.UpdateEntityAsync(viajeOriginal);

            if (result.Success)
            {
                await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                {
                    ActorId = 1,
                    ModuloAfectado = "Operaciones",
                    AccionRealizada = "Actualización de Estado",
                    Detalles = $"Viaje #{viajeId} pasó de '{estadoAnterior}' a '{nuevoEstado}'.",
                    ResultadoExitoso = true,
                    CreationDate = DateTime.Now
                });
            }
            return result;
        }
    }
}