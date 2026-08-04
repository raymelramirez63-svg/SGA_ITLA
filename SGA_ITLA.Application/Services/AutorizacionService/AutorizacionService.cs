using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.Autorizaciones
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly IAutorizacionRepository _autorizacionRepo;
        private readonly IAuditoriaRepository _auditoriaRepo;

        public AutorizacionService(IAutorizacionRepository autorizacionRepo, IAuditoriaRepository auditoriaRepo)
        {
            _autorizacionRepo = autorizacionRepo;
            _auditoriaRepo = auditoriaRepo;
        }

        public async Task<OperationResult> EmitirTicketMensualAsync(int usuarioId, int pagoId, DateTime fechaInicio)
        {
            // Verificacion de si ya tiene algo activo
            var autorizacionExistente = await _autorizacionRepo.ObtenerAutorizacionActivaPorUsuarioAsync(usuarioId);
            if (autorizacionExistente != null && autorizacionExistente.Tipo == TipoAutorizacion.TicketMensual)
            {
                return new OperationResult { Success = false, Message = "El usuario ya posee un ticket mensual activo." };
            }

            var nuevaAut = new Autorizacion
            {
                UsuarioId = usuarioId,
                Tipo = TipoAutorizacion.TicketMensual,
                FechaInicioVigencia = fechaInicio,
                FechaFinVigencia = fechaInicio.AddMonths(1), // Automáticamente dura 1 mes
                IsActive = true
            };

            var result = await _autorizacionRepo.SaveEntityAsync(nuevaAut);

            if (result.Success)
            {
                // Cumplimiento Regla RN-AUD: Trazabilidad inmutable
                await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                {
                    ActorId = 1, // ID del sistema
                    ModuloAfectado = "Autorizaciones",
                    AccionRealizada = "Emisión Ticket Mensual",
                    Detalles = $"Pago ID #{pagoId}. Válido hasta {nuevaAut.FechaFinVigencia?.ToString("dd/MM/yyyy")}",
                    ResultadoExitoso = true,
                    CreationDate = DateTime.Now
                });

                result.Message = $"Ticket mensual emitido. Válido hasta {nuevaAut.FechaFinVigencia?.ToString("dd/MM/yyyy")}.";
            }

            return result;
        }

        public async Task<OperationResult> RecargarTarjetaAsync(int usuarioId, decimal monto)
        {
            var autorizacion = await _autorizacionRepo.ObtenerAutorizacionActivaPorUsuarioAsync(usuarioId);

            // Si el usuario no tiene una tarjeta, se la creamos desde cero con el balance
            if (autorizacion == null)
            {
                var nuevaTarjeta = new Autorizacion
                {
                    UsuarioId = usuarioId,
                    Tipo = TipoAutorizacion.TarjetaRecargable,
                    SaldoDisponible = monto,
                    IsActive = true
                };

                var result = await _autorizacionRepo.SaveEntityAsync(nuevaTarjeta);
                if (result.Success)
                {
                    await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                    {
                        ActorId = 1,
                        ModuloAfectado = "Autorizaciones",
                        AccionRealizada = "Emisión Tarjeta Nueva",
                        Detalles = $"Balance inicial: RD${monto}",
                        ResultadoExitoso = true,
                        CreationDate = DateTime.Now
                    });
                    result.Message = $"Tarjeta nueva emitida con saldo de RD${monto}.";
                }
                return result;
            }

            // RN-PAG: Si tiene un Ticket Mensual, no se recarga dinero
            if (autorizacion.Tipo != TipoAutorizacion.TarjetaRecargable)
            {
                return new OperationResult { Success = false, Message = "Rechazado: El usuario posee un Ticket Mensual. No admite recarga de saldo." };
            }

            //  sumamos el dinero real
            autorizacion.SaldoDisponible = (autorizacion.SaldoDisponible ?? 0) + monto;
            var updateResult = await _autorizacionRepo.UpdateEntityAsync(autorizacion);

            if (updateResult.Success)
            {
                // Cumplimiento Regla RN-AUD: Guardamos el log de la recarga
                await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                {
                    ActorId = 1,
                    ModuloAfectado = "Autorizaciones",
                    AccionRealizada = "Recarga de Tarjeta",
                    Detalles = $"Abono: RD${monto}. Nuevo saldo total: RD${autorizacion.SaldoDisponible}",
                    ResultadoExitoso = true,
                    CreationDate = DateTime.Now
                });

                updateResult.Message = $"Recarga procesada exitosamente. Nuevo saldo disponible: RD${autorizacion.SaldoDisponible}";
            }

            return updateResult;
        }
    }
}