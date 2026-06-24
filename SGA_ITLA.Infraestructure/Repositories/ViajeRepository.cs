using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Base;
using SGA_ITLA.Infraestructure.Context;

namespace SGA_ITLA.Infraestructure.Repositories
{
    public class ViajeRepository : BaseRepository<Viaje>, IViajeRepository
    {
        public ViajeRepository(SgaContext context) : base(context) { }

        public async Task<OperationResult> GetViajesDetalladosAsync()
        {
            var result = new OperationResult();
            try
            {
                result.Data = await _context.Viajes
                    .Include(v => v.Ruta)
                    .Include(v => v.Autobus)
                    .Include(v => v.Conductor)
                    .ToListAsync();
            }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }
    }
}