using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Interfaces.Autorizaciones
{
    public interface ISolicitudService
    {
        Task<OperationResult> CrearSolicitudAsync(int usuarioId, TipoAutorizacion tipo, string? comentario);
        Task<OperationResult> AprobarSolicitudAsync(int solicitudId, int pagoId, decimal? monto);
        Task<OperationResult> RechazarSolicitudAsync(int solicitudId, string motivo);
    }
}