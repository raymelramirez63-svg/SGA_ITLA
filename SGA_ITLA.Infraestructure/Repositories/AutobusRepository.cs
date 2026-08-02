using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class AutobusRepository : BaseRepository<Autobus>, IAutobusRepository
    {
        public AutobusRepository(SgaContext context) : base(context) { }

        public async Task<int> ObtenerCapacidadMaximaAsync(int autobusId)
        {
            return await _context.Autobuses
                .Where(a => a.Id == autobusId)
                .Select(a => a.CapacidadMaxima)
                .FirstOrDefaultAsync();
        }
    }
}