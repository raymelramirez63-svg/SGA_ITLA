using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IViajeRepository : IBaseRepository<Viaje>
    {
        Task<OperationResult> GetViajesDetalladosAsync();
        Task<bool> ExisteConflictoDeRecursosAsync(int autobusId, int conductorId, DateTime fechaPlanificada);
        Task<IEnumerable<Viaje>> ObtenerViajesDelDiaAsync(DateTime fecha);
        Task<IEnumerable<Viaje>> ObtenerViajesPorConductorAsync(int conductorId);

        Task<bool> AutobusTieneViajeActivoAsync(int autobusId, DateTime fechaPlanificada);
        Task<bool> ConductorTieneViajeActivoAsync(int conductorId, DateTime fechaPlanificada);

        Task<bool> RutaTieneViajesActivosAsync(int rutaId);
        Task<bool> ConductorTieneViajesActivosGlobalAsync(int conductorId);
    }
}