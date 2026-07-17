using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Interfaces; 

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<bool> ExisteIdentificacionAsync(string identificacion);
    }
}