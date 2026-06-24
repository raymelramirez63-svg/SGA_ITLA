using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Services.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CatalogoController : ControllerBase
{
    private readonly CatalogoService _service;
    public CatalogoController(CatalogoService service) => _service = service;

    [HttpPost("autobus")]
    public async Task<IActionResult> RegistrarAutobus(Autobus autobus)
        => Ok(await _service.RegistrarAutobusAsync(autobus));

    [HttpGet("rutas")]
    public async Task<IActionResult> GetRutas()
        => Ok(await _service.ObtenerRutasAsync());
}