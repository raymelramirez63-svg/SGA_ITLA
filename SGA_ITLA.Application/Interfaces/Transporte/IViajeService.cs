using System.Threading.Tasks;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Application.Interfaces.Transporte
{
    public interface IViajeService
    {
        Task<OperationResult> GetAllViajesActivosAsync();
        Task<OperationResult> SaveViajeAsync(SaveViajeDto saveViajeDto);
    }
}