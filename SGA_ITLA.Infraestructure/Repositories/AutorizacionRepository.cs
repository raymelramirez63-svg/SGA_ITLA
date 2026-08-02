using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class AutorizacionRepository : BaseRepository<Autorizacion>, IAutorizacionRepository
    {
        public AutorizacionRepository(SgaContext context) : base(context) { }

        public async Task<Autorizacion?> ObtenerAutorizacionActivaPorUsuarioAsync(int usuarioId)
        {
            return await _context.Autorizaciones
                .Where(a => a.UsuarioId == usuarioId && a.IsActive == true &&
                           (a.FechaFinVigencia == null || a.FechaFinVigencia >= DateTime.Now))
                .FirstOrDefaultAsync();
        }
    }
}