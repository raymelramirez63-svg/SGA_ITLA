using System;
using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Domain.Entities.Auditoria
{
    public class RegistroAuditoria : BaseEntity
    {
        public int ActorId { get; set; }
        public string ModuloAfectado { get; set; } = string.Empty;
        public string AccionRealizada { get; set; } = string.Empty;
        public string Detalles { get; set; } = string.Empty;
        public bool ResultadoExitoso { get; set; }
        public string MotivoFallo { get; set; } = string.Empty;

        public new int? ModifyUser { get; private set; }
        public new DateTime? ModifyDate { get; private set; }
        public new int? DeleteUser { get; private set; }
        public new DateTime? DeleteDate { get; private set; }
        public new bool Deleted { get; private set; }
    }
}