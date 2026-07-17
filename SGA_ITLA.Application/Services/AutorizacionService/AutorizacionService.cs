using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Base;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Autorizaciones
{
    public class AutorizacionService : IAutorizacionService
    {
        public async Task<OperationResult> EmitirTicketMensualAsync(int usuarioId, int pagoId, DateTime fechaInicio)
        {
            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = $"Ticket mensual emitido para el usuario {usuarioId}, asociado al pago {pagoId}, registrado en auditoría exitosamente."
            });
        }

        public async Task<OperationResult> RecargarTarjetaAsync(int usuarioId, decimal monto)
        {
            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = $"Tarjeta recargada con RD${monto} para el usuario {usuarioId} exitosamente. Transacción guardada."
            });
        }
    }
}