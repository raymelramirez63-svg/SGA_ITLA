using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Persistence.Interfaces.Transporte
{
    public interface IViajeRepository : IBaseRepository<Viaje>
    {
        Task<OperationResult> GetViajesActivosAsync();
    }
}