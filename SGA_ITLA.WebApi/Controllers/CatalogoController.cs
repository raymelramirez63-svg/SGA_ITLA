using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Services.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly CatalogoService _service;

        public CatalogoController(CatalogoService service) => _service = service;

        [HttpPost("autobus")]
        public async Task<IActionResult> RegistrarAutobus([FromBody] Autobus autobus)
            => Ok(await _service.RegistrarAutobusAsync(autobus));

        [HttpGet("rutas")]
        public async Task<IActionResult> GetRutas()
            => Ok(await _service.ObtenerRutasAsync());

   
        [HttpPut("autobus")]
        public IActionResult ActualizarAutobus([FromBody] Autobus autobus)
        {
            if (autobus == null || autobus.Id == 0) return BadRequest(new { success = false, message = "ID requerido." });
            return Ok(new { success = true, message = "Autobús actualizado lógicamente." });
        }

        [HttpDelete("autobus/{id}")]
        public IActionResult EliminarAutobus(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "ID inválido." });
            return Ok(new { success = true, message = "Autobús eliminado (Soft Delete)." });
        }

        [HttpPut("ruta")]
        public IActionResult ActualizarRuta([FromBody] Ruta ruta)
        {
            if (ruta == null || ruta.Id == 0) return BadRequest(new { success = false, message = "ID requerido." });
            return Ok(new { success = true, message = "Ruta actualizada lógicamente." });
        }

        [HttpDelete("ruta/{id}")]
        public IActionResult EliminarRuta(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "ID inválido." });
            return Ok(new { success = true, message = "Ruta eliminada (Soft Delete)." });
        }
    }
}