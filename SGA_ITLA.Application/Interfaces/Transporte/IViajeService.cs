using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Application.Interfaces.Transporte
{
    public interface IViajeService
    {
        Task<OperationResult> ObtenerViajesDetalladosAsync();
        Task<OperationResult> RegistrarViajeAsync(Viaje viaje);
    }
}