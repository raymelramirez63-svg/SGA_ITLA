using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IViajeRepository : IBaseRepository<Viaje>
    {
        Task<OperationResult> GetViajesDetalladosAsync();
    }
}