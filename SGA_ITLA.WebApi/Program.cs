using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Persistence.Context;
using SGA_ITLA.IOC;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SgaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SgaDb")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSgaDependencies();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();