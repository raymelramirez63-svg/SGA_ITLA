using Microsoft.Extensions.Logging;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
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
            try
            {
                _logger.LogInformation("Intentando registrar un nuevo viaje en el sistema...");
                return await _viajeRepo.SaveEntityAsync(viaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error crítico al registrar el viaje.");
                return new OperationResult { Success = false, Message = "Error interno al registrar el viaje." };
            }
        }
    }
}