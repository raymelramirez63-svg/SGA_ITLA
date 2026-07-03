using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Repositories;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Services.Transporte;

namespace SGA_ITLA.WebApi.Dependencias
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSgaDependencies(this IServiceCollection services, string connectionString)
        {
           
            services.AddDbContext<SgaContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAutorizacionRepository, AutorizacionRepository>();
            services.AddScoped<IAutobusRepository, AutobusRepository>();
            services.AddScoped<IRutaRepository, RutaRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IViajeRepository, ViajeRepository>();

            services.AddScoped<IViajeService, ViajeService>();

            return services;
        }
    }
}