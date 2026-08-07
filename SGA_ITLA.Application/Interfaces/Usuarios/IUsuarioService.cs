using SGA_ITLA.Domain.Entities.Usuarios;
using System.Threading.Tasks;

namespace SGA_ITLA.Application.Interfaces.Usuarios
{
    public interface IUsuarioService
    {
        Task<Usuario?> ValidarCredencialesAsync(string email, string password);
    }
}