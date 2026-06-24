using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SGA_ITLA.Infraestructure.Context
{
    public class SgaContextFactory : IDesignTimeDbContextFactory<SgaContext>
    {
        public SgaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SgaContext>();

            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SGA_ITLA_Oficial_DB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            return new SgaContext(optionsBuilder.Options);
        }
    }
}