using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Enums;
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

        public async Task<bool> ExisteConflictoDeRecursosAsync(int autobusId, int conductorId, DateTime fechaPlanificada)
        {
            return await _context.Viajes.AnyAsync(v =>
                (v.AutobusId == autobusId || v.ConductorId == conductorId) &&
                v.HorarioSalidaPlanificada.Date == fechaPlanificada.Date &&
                (v.Estado == EstadoViaje.Programado || v.Estado == EstadoViaje.EnCurso));
        }

        public async Task<IEnumerable<Viaje>> ObtenerViajesDelDiaAsync(DateTime fecha)
        {
            return await _context.Viajes
                .Include(v => v.Ruta)
                .Include(v => v.Autobus)
                .Where(v => v.HorarioSalidaPlanificada.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Viaje>> ObtenerViajesPorConductorAsync(int conductorId)
        {
            return await _context.Viajes
                .Include(v => v.Ruta)
                .Where(v => v.ConductorId == conductorId &&
                           (v.Estado == EstadoViaje.Programado || v.Estado == EstadoViaje.EnCurso))
                .ToListAsync();
        }
    }
}