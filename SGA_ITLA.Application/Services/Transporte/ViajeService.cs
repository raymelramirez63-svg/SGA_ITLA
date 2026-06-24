using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Persistence.Interfaces;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class ViajeService : IViajeService
    {
        private readonly IViajeRepository _viajeRepository;
        private readonly ILogger<ViajeService> _logger;

        public ViajeService(IViajeRepository viajeRepository, ILogger<ViajeService> logger)
        {
            _viajeRepository = viajeRepository;
            _logger = logger;
        }

        public async Task<OperationResult> GetAllViajesActivosAsync()
        {
            return await _viajeRepository.GetAllAsync();
        }

        public async Task<OperationResult> SaveViajeAsync(SaveViajeDto saveViajeDto)
        {
            OperationResult result = new OperationResult();
            try
            {
                var nuevoViaje = new Viaje
                {
                    RutaId = saveViajeDto.RutaId,
                    AutobusId = saveViajeDto.AutobusId,
                    ConductorId = saveViajeDto.ConductorId,
                    HorarioSalidaPlanificada = saveViajeDto.HorarioSalida,
                    Estado = saveViajeDto.EstadoViaje,
                    CreationUser = 1
                };
                result = await _viajeRepository.SaveEntityAsync(nuevoViaje);
            }
            catch (Exception ex)
            {
                result.Success = false;
                _logger.LogError(ex, "Error al guardar el viaje.");
            }
            return result;
        }
    }
}