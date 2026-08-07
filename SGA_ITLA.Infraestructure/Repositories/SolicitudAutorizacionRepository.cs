using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class SolicitudAutorizacionRepository : BaseRepository<SolicitudAutorizacion>, ISolicitudAutorizacionRepository
    {
        public SolicitudAutorizacionRepository(SgaContext context) : base(context) { }

        public async Task<IEnumerable<SolicitudAutorizacion>> ObtenerPendientesAsync()
        {
            return await _context.Set<SolicitudAutorizacion>()
                .Include(s => s.Usuario)
                .Where(s => s.Estado == EstadoSolicitud.Pendiente)
                .OrderBy(s => s.CreationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitudAutorizacion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Set<SolicitudAutorizacion>()
                .Where(s => s.UsuarioId == usuarioId)
                .OrderByDescending(s => s.CreationDate)
                .ToListAsync();
        }
    }
}