using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.Domain.Entities.Usuarios
{
    public class Usuario : BaseEntity
    {
        public string IdentificacionInstitucional { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public bool IsActive { get; set; } = true;
    }
}