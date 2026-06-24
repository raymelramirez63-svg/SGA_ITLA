using Microsoft.Extensions.DependencyInjection;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Services.Transporte;
using SGA_ITLA.Persistence.Interfaces;
using SGA_ITLA.Persistence.Repositories;

namespace SGA_ITLA.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSgaDependencies(this IServiceCollection services)
        {
     
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAutorizacionRepository, AutorizacionRepository>();
            services.AddScoped<IAutobusRepository, AutobusRepository>();
            services.AddScoped<IRutaRepository, RutaRepository>();
            services.AddScoped<IViajeRepository, ViajeRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();

            services.AddScoped<IViajeService, ViajeService>();

            return services;
        }
    }
}