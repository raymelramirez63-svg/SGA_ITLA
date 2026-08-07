using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<bool> ExisteIdentificacionAsync(string identificacion);
        Task<Usuario> GetByEmailAsync(string email);

        Task<Usuario?> ObtenerPorIdentificacionAsync(string identificacion);
    }
}