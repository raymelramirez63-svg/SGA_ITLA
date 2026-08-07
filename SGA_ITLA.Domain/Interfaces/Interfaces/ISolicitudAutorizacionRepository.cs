using System.Collections.Generic;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Autorizaciones;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface ISolicitudAutorizacionRepository : IBaseRepository<SolicitudAutorizacion>
    {
        Task<IEnumerable<SolicitudAutorizacion>> ObtenerPendientesAsync();
        Task<IEnumerable<SolicitudAutorizacion>> ObtenerPorUsuarioAsync(int usuarioId);
    }
}