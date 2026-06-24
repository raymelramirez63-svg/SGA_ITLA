using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.WebApi.Dependencias; 

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SgaDb")!;

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();