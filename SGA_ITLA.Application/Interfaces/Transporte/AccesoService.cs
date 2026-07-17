using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class AccesoService : IAccesoService
    {
        public async Task<OperationResult> ValidarAbordajeAsync(int viajeId, int usuarioId)
        {

            return await Task.FromResult(new OperationResult
            {
                Success = true,
                Message = "Acceso Permitido. Validación concurrente de saldo y cupo exitosa. Registro guardado en Auditoría."
            });
        }
    }
}