using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Application.Services.Catalogo;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Repositories;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Services.Transporte;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Application.Services.Autorizaciones;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. CONEXIÓN A LA BASE DE DATOS DIRECTA (In-Process)
var connectionString = builder.Configuration.GetConnectionString("SgaDb");
builder.Services.AddDbContext<SgaContext>(options =>
    options.UseSqlServer(connectionString));

// 2. CONFIGURACIÓN DE SEGURIDAD POR COOKIES (Para evitar el 404)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Si no está logueado, lo manda aquí
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// 3. INYECCIÓN DE REPOSITORIOS (Capa de Infraestructura)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutorizacionRepository, AutorizacionRepository>();
builder.Services.AddScoped<IAutobusRepository, AutobusRepository>();
builder.Services.AddScoped<IRutaRepository, RutaRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IViajeRepository, ViajeRepository>();
builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();

// 4. INYECCIÓN DE SERVICIOS (Capa de Aplicación)
builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();