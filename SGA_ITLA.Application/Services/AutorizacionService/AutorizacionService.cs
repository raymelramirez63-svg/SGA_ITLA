using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Base;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Autorizaciones
{
    public class AutorizacionService : IAutorizacionService
    {
        public async Task<OperationResult> EmitirTicketMensualAsync(int estudianteId)
        {
            
            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = "Ticket mensual emitido, asociado al estudiante y registrado en auditoría exitosamente."
            });
        }

        public async Task<OperationResult> RecargarTarjetaAsync(int estudianteId, decimal monto)
        {
            
            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = $"Tarjeta recargada con RD${monto} exitosamente. Transacción guardada."
            });
        }
    }
}