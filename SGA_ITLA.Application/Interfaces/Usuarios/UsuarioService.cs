using System.Threading.Tasks;
using SGA_ITLA.Application.Interfaces.Usuarios;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Interfaces;

namespace SGA_ITLA.Application.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string email, string password)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash) && usuario.IsActive)
            {
                return usuario;
            }

            return null;
        }
    }
}