using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Application.Services.Autorizaciones;
using SGA_ITLA.Application.Services.AutorizacionService;
using SGA_ITLA.Application.Services.Catalogo;
using SGA_ITLA.Application.Services.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Infraestructure.Repositories;
using SGA_ITLA.WebMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("SgaDb");
builder.Services.AddDbContext<SgaContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddHttpContextAccessor();

// Registro del interceptador
builder.Services.AddTransient<AuthTokenHandler>();

// 1. Cliente LIMPIO (Solo para el AuthApiService, no pide token)
builder.Services.AddHttpClient("SgaApiClean", client => {
    client.BaseAddress = new Uri("https://localhost:7031/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("SgaApi", client => {
    client.BaseAddress = new Uri("https://localhost:7031/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<ICatalogoApiService, CatalogoApiService>();
builder.Services.AddScoped<ITransporteApiService, TransporteApiService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAutorizacionRepository, AutorizacionRepository>();
builder.Services.AddScoped<IAutobusRepository, AutobusRepository>();
builder.Services.AddScoped<IRutaRepository, RutaRepository>();
builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IViajeRepository, ViajeRepository>();
builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
builder.Services.AddScoped<ISolicitudAutorizacionRepository, SolicitudAutorizacionRepository>();

builder.Services.AddScoped<ICatalogoService, CatalogoService>();
builder.Services.AddScoped<IViajeService, ViajeService>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();

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