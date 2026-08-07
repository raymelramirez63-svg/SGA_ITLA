using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Services.AutorizacionService
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudAutorizacionRepository _solicitudRepo;
        private readonly IAutorizacionService _autorizacionService;
        private readonly IAuditoriaRepository _auditoriaRepo;

        public SolicitudService(ISolicitudAutorizacionRepository solicitudRepo, IAutorizacionService autorizacionService, IAuditoriaRepository auditoriaRepo)
        {
            _solicitudRepo = solicitudRepo;
            _autorizacionService = autorizacionService;
            _auditoriaRepo = auditoriaRepo;
        }

        public async Task<OperationResult> CrearSolicitudAsync(int usuarioId, TipoAutorizacion tipo, string? comentario)
        {
            if (tipo == TipoAutorizacion.TicketMensual)
            {
                var misSolicitudes = await _solicitudRepo.ObtenerPorUsuarioAsync(usuarioId);
                bool tieneTicketPendiente = misSolicitudes.Any(s =>
                    s.TipoSolicitado == TipoAutorizacion.TicketMensual &&
                    s.Estado == EstadoSolicitud.Pendiente);

                if (tieneTicketPendiente)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = "Ya tienes una solicitud de Ticket Mensual en proceso. Por favor, espera a que sea evaluada."
                    };
                }
            }

            var solicitud = new SolicitudAutorizacion
            {
                UsuarioId = usuarioId,
                TipoSolicitado = tipo,
                Comentario = comentario,
                Estado = EstadoSolicitud.Pendiente
            };

            var result = await _solicitudRepo.SaveEntityAsync(solicitud);
            if (result.Success)
            {
                await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                {
                    ActorId = usuarioId,
                    ModuloAfectado = "Autorizaciones",
                    AccionRealizada = "Solicitud de Autorización Creada",
                    Detalles = $"Tipo solicitado: {tipo}.",
                    ResultadoExitoso = true,
                    CreationDate = DateTime.Now
                });
                result.Message = "Solicitud enviada. Un administrador de autorizaciones la revisará pronto.";
            }
            return result;
        }

        public async Task<OperationResult> AprobarSolicitudAsync(int solicitudId, int pagoId, decimal? monto)
        {
            var solicitudResult = await _solicitudRepo.GetByIdAsync(solicitudId);
            if (!solicitudResult.Success || solicitudResult.Data is not SolicitudAutorizacion solicitud)
                return new OperationResult { Success = false, ErrorType = "NotFound", Message = "Solicitud no encontrada." };

            if (solicitud.Estado != EstadoSolicitud.Pendiente)
                return new OperationResult { Success = false, ErrorType = "EstadoInvalido", Message = "Esta solicitud ya fue procesada." };

            var resultadoEmision = solicitud.TipoSolicitado == TipoAutorizacion.TicketMensual
                ? await _autorizacionService.EmitirTicketMensualAsync(solicitud.UsuarioId, pagoId, DateTime.Today)
                : await _autorizacionService.RecargarTarjetaAsync(solicitud.UsuarioId, monto ?? 0m);

            if (!resultadoEmision.Success) return resultadoEmision;

            solicitud.Estado = EstadoSolicitud.Aprobada;
            await _solicitudRepo.UpdateEntityAsync(solicitud);

            return new OperationResult { Success = true, Message = "Solicitud aprobada y autorización emitida." };
        }

        public async Task<OperationResult> RechazarSolicitudAsync(int solicitudId, string motivo)
        {
            var solicitudResult = await _solicitudRepo.GetByIdAsync(solicitudId);
            if (!solicitudResult.Success || solicitudResult.Data is not SolicitudAutorizacion solicitud)
                return new OperationResult { Success = false, ErrorType = "NotFound", Message = "Solicitud no encontrada." };

            solicitud.Estado = EstadoSolicitud.Rechazada;
            solicitud.MotivoRechazo = motivo;
            var result = await _solicitudRepo.UpdateEntityAsync(solicitud);
            result.Message = "Solicitud rechazada.";
            return result;
        }
    }
}