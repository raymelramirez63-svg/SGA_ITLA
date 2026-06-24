using Microsoft.EntityFrameworkCore;
using System.Linq; // <-- Asegúrate de que esto esté aquí para que funcione la magia
using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;

namespace SGA_ITLA.Persistence.Context
{
    public class SgaContext : DbContext
    {
        public SgaContext(DbContextOptions<SgaContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Autorizacion> Autorizaciones { get; set; }
        public DbSet<Autobus> Autobuses { get; set; }
        public DbSet<Ruta> Rutas { get; set; }
        public DbSet<Parada> Paradas { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Viaje> Viajes { get; set; }
        public DbSet<Incidencia> Incidencias { get; set; }
        public DbSet<RegistroAuditoria> RegistrosAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Autorizacion>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Autobus>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Ruta>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<Viaje>().HasQueryFilter(x => !x.Deleted);
            modelBuilder.Entity<RegistroAuditoria>().HasQueryFilter(x => !x.Deleted);

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}