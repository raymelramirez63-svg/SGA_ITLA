using SGA_ITLA.Domain.Base;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Interfaces.Autorizaciones
{
    public interface IAutorizacionService
    {
        Task<OperationResult> EmitirTicketMensualAsync(int estudianteId);
        Task<OperationResult> RecargarTarjetaAsync(int estudianteId, decimal monto);
    }
}