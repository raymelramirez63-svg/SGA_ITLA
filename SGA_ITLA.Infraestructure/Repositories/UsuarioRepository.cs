using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;
using System.Threading.Tasks;

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SgaContext context) : base(context)
        {
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion)
        {
            return await _context.Set<Usuario>()
                .AnyAsync(u => u.IdentificacionInstitucional == identificacion);
        }
    }
}