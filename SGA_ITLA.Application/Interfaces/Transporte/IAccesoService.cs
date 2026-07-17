using SGA_ITLA.Domain.Base;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Interfaces.Transporte
{
    public interface IAccesoService
    {
        Task<OperationResult> ValidarAbordajeAsync(int viajeId, int usuarioId);
    }
}