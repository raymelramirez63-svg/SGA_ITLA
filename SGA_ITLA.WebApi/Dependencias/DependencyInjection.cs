using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Application.Services.Catalogo;
using SGA_ITLA.Application.Services.Transporte;
using SGA_ITLA.Application.Services.Autorizaciones;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Infraestructure.Repositories;

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

         
            services.AddScoped<ICatalogoService, CatalogoService>();
            services.AddScoped<IViajeService, ViajeService>();
            services.AddScoped<IAccesoService, AccesoService>();
            services.AddScoped<IAutorizacionService, AutorizacionService>();

            return services;
        }
    }
}