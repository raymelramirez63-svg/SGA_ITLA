using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Domain.Entities.Transporte;

namespace SGA_ITLA.Persistence.Context
{
    public class SgaContext : DbContext
    {
        public SgaContext(DbContextOptions<SgaContext> options) : base(options)
        {
        }

        public DbSet<Autobus> Autobuses { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Viaje> Viajes { get; set; }
    }
}