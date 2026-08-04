using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Entities.Auditoria;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Transporte
{
    public class AccesoService : IAccesoService
    {
        private readonly IViajeRepository _viajeRepo;
        private readonly IAutobusRepository _autobusRepo;
        private readonly IAutorizacionRepository _autorizacionRepo;
        private readonly IAuditoriaRepository _auditoriaRepo; 

        public AccesoService(
            IViajeRepository viajeRepo,
            IAutobusRepository autobusRepo,
            IAutorizacionRepository autorizacionRepo,
            IAuditoriaRepository auditoriaRepo)
        {
            _viajeRepo = viajeRepo;
            _autobusRepo = autobusRepo;
            _autorizacionRepo = autorizacionRepo;
            _auditoriaRepo = auditoriaRepo;
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

            // PROCESO DE COBRO (SI ES TARJETA)
            string detalleAuditoria = $"Viaje #{viajeId}. Acceso mediante Ticket Mensual.";

            if (autorizacion.Tipo == TipoAutorizacion.TarjetaRecargable)
            {
                decimal costoViaje = 50.00m;
                decimal saldoActual = autorizacion.SaldoDisponible ?? 0m;

                if (saldoActual < costoViaje)
                {
                    // Auditar el rechazo por falta de fondos
                    await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria { ActorId = usuarioId, ModuloAfectado = "Control de Acceso", AccionRealizada = "Intento de Abordaje", Detalles = $"Rechazado. Saldo insuficiente (RD${saldoActual}).", ResultadoExitoso = false, CreationDate = DateTime.Now });
                    return new OperationResult { Success = false, Message = "Rechazado (RN-PAG): Saldo insuficiente en la tarjeta recargable." };
                }

                autorizacion.SaldoDisponible = saldoActual - costoViaje;
                await _autorizacionRepo.UpdateEntityAsync(autorizacion);
                detalleAuditoria = $"Viaje #{viajeId}. Cobro: RD${costoViaje}. Saldo restante: RD${autorizacion.SaldoDisponible}";
            }

            viaje.CupoDisponibleActual -= 1;
            await _viajeRepo.UpdateEntityAsync(viaje);

            await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
            {
                ActorId = usuarioId,
                ModuloAfectado = "Control de Acceso",
                AccionRealizada = "Abordaje Exitoso",
                Detalles = detalleAuditoria,
                ResultadoExitoso = true,
                CreationDate = DateTime.Now
            });

            return new OperationResult
            {
                Success = true,
                Message = "Acceso Permitido. Validación concurrente exitosa, saldo descontado y cupo actualizado en la Base de Datos."
            };
        }
    }
}