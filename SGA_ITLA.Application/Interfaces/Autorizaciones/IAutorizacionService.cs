using SGA_ITLA.Domain.Base;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Interfaces.Autorizaciones
{
    public interface IAutorizacionService
    {
        Task<OperationResult> EmitirTicketMensualAsync(int usuarioId, int pagoId, DateTime fechaInicio);
        Task<OperationResult> RecargarTarjetaAsync(int usuarioId, decimal monto);
    }
}