using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Domain.Entities.Autorizaciones
{
    public class SolicitudAutorizacion : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public TipoAutorizacion TipoSolicitado { get; set; }
        public string? Comentario { get; set; }
        public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
        public string? MotivoRechazo { get; set; }
    }
}