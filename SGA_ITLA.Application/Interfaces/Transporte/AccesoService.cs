using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class AccesoService : IAccesoService
    {
        private readonly IViajeRepository _viajeRepo;
        private readonly IAutobusRepository _autobusRepo;
        private readonly IAutorizacionRepository _autorizacionRepo;

        public AccesoService(
            IViajeRepository viajeRepo,
            IAutobusRepository autobusRepo,
            IAutorizacionRepository autorizacionRepo)
        {
            _viajeRepo = viajeRepo;
            _autobusRepo = autobusRepo;
            _autorizacionRepo = autorizacionRepo;
        }

        public async Task<OperationResult> ValidarAbordajeAsync(int viajeId, int usuarioId)
        {
            var viajeResult = await _viajeRepo.GetByIdAsync(viajeId);
            if (!viajeResult.Success || viajeResult.Data == null)
                return new OperationResult { Success = false, Message = "Rechazado: El viaje especificado no existe." };

           
            if (viajeResult.Data is not Viaje viaje)
                return new OperationResult { Success = false, Message = "Rechazado: Datos de viaje inválidos." };

            if (viaje.Estado != EstadoViaje.EnCurso)
                return new OperationResult { Success = false, Message = "Rechazado: El viaje aún no está en curso o ya finalizó." };

            if (viaje.CupoDisponibleActual <= 0)
                return new OperationResult { Success = false, Message = "Rechazado (RN-ACC): El autobús ha alcanzado su capacidad máxima." };

            var autorizacion = await _autorizacionRepo.ObtenerAutorizacionActivaPorUsuarioAsync(usuarioId);

            if (autorizacion == null)
                return new OperationResult { Success = false, Message = "Rechazado (RN-ACC): No posee una autorización vigente o activa." };

            if (autorizacion.Tipo == TipoAutorizacion.TarjetaRecargable)
            {
                decimal costoViaje = 50.00m;

                decimal saldoActual = autorizacion.SaldoDisponible ?? 0m;

                if (saldoActual < costoViaje)
                    return new OperationResult { Success = false, Message = "Rechazado (RN-PAG): Saldo insuficiente en la tarjeta recargable." };

                autorizacion.SaldoDisponible = saldoActual - costoViaje;
                await _autorizacionRepo.UpdateEntityAsync(autorizacion);
            }

            viaje.CupoDisponibleActual -= 1;
            await _viajeRepo.UpdateEntityAsync(viaje);

            return new OperationResult
            {
                Success = true,
                Message = "Acceso Permitido. Validación concurrente exitosa, saldo descontado y cupo actualizado en la Base de Datos."
            };
        }
    }
}