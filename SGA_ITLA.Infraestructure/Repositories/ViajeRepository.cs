using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Persistence.Base;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.Persistence.Interfaces.Transporte;

namespace SGA_ITLA.Persistence.Repositories.Transporte
{
    public class ViajeRepository : BaseRepository<Viaje>, IViajeRepository
    {
        private readonly SgaContext _context;
        private readonly ILogger<ViajeRepository> _logger;
        private readonly IConfiguration _configuration;

        public ViajeRepository(SgaContext context,
                               ILogger<ViajeRepository> logger,
                               IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<OperationResult> GetViajesActivosAsync()
        {
            OperationResult result = new OperationResult();
            try
            {
                var query = await _context.Viajes
                                          .Include(v => v.Ruta)
                                          .Include(v => v.Autobus)
                                          .Where(v => v.Deleted == false)
                                          .ToListAsync();

                result.Data = query;
            }
            catch (Exception ex)
            {
                result.Message = _configuration["TransporteError:GetViajesActivos"] ?? "Error obteniendo los viajes por departamentos.";
                result.Success = false;
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}