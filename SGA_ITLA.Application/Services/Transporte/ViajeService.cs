using Microsoft.Extensions.Logging;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                _logger.LogInformation("Solicitando la lista detallada de viajes a la base de datos...");
                return await _viajeRepo.GetViajesDetalladosAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error crítico al intentar obtener los viajes.");
                return new OperationResult { Success = false, Message = "Error interno al obtener viajes." };
            }
        }

        public async Task<OperationResult> RegistrarViajeAsync(Viaje viaje)
        {
            var result = new OperationResult();
            try
            {
                _logger.LogInformation("Aplicando reglas de negocio para la planificación del viaje...");

                if (viaje.AutobusId <= 0 || viaje.ConductorId <= 0 || viaje.RutaId <= 0)
                {
                    result.Success = false;
                    result.Message = "Ruta, Autobús y Conductor son obligatorios para planificar un viaje.";
                    return result;
                }

                var viajesActualesResult = await _viajeRepo.GetAllAsync();

                if (viajesActualesResult.Success && viajesActualesResult.Data != null)
                {
                    var viajesActivos = (IEnumerable<Viaje>)viajesActualesResult.Data;

                    bool autobusOcupado = viajesActivos.Any(v =>
                        v.AutobusId == viaje.AutobusId && ((int)v.Estado == 1 || (int)v.Estado == 2));

                    if (autobusOcupado)
                    {
                        result.Success = false;
                        result.Message = "Conflicto: El autobús ya está asignado a otro viaje activo en este momento.";
                        return result;
                    }

                    bool conductorOcupado = viajesActivos.Any(v =>
                        v.ConductorId == viaje.ConductorId && ((int)v.Estado == 1 || (int)v.Estado == 2));

                    if (conductorOcupado)
                    {
                        result.Success = false;
                        result.Message = "Conflicto: El conductor ya está asignado a otro viaje activo en este momento.";
                        return result;
                    }
                }

                viaje.Estado = (SGA_ITLA.Domain.Enums.EstadoViaje)1;

                _logger.LogInformation("Reglas de negocio validadas sin conflictos. Guardando viaje...");
                return await _viajeRepo.SaveEntityAsync(viaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error crítico al registrar el viaje.");
                result.Success = false;
                result.Message = "Error interno al registrar el viaje.";
                return result;
            }
        }
    }
}