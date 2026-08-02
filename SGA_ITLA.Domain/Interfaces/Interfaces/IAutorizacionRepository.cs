using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Autorizaciones;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IAutorizacionRepository : IBaseRepository<Autorizacion>
    {
        Task<Autorizacion?> ObtenerAutorizacionActivaPorUsuarioAsync(int usuarioId);
    }
}