using Microsoft.Extensions.DependencyInjection;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Services.Transporte;
using SGA_ITLA.Persistence.Interfaces.Transporte;
using SGA_ITLA.Persistence.Repositories.Transporte;

namespace SGA_ITLA.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSgaDependencies(this IServiceCollection services)
        {
           
            services.AddScoped<IViajeRepository, ViajeRepository>();

          
            services.AddScoped<IViajeService, ViajeService>();

            return services;
        }
    }
}