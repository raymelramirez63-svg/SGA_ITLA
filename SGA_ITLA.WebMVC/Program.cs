using Microsoft.AspNetCore.Authentication.Cookies;
using SGA_ITLA.WebMVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddHttpContextAccessor();

// Registro del interceptador JWT
builder.Services.AddTransient<AuthTokenHandler>();

// Cliente LIMPIO para Auth (No envía token)
builder.Services.AddHttpClient("SgaApiClean", client => {
    client.BaseAddress = new Uri("https://localhost:7031/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Cliente PROTEGIDO para consumir la API con el token de sesión
builder.Services.AddHttpClient("SgaApi", client => {
    client.BaseAddress = new Uri("https://localhost:7031/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped<IAuthApiService, AuthApiService>();
builder.Services.AddScoped<ICatalogoApiService, CatalogoApiService>();
builder.Services.AddScoped<ITransporteApiService, TransporteApiService>();

builder.Services.AddScoped<IAuditoriaApiService, AuditoriaApiService>();
builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
builder.Services.AddScoped<IAutorizacionApiService, AutorizacionApiService>();
builder.Services.AddScoped<ISolicitudApiService, SolicitudApiService>();

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