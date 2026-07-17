using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Transporte.Viajes; 
using SGA_ITLA.Application.Interfaces.Transporte;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly IAccesoService _accesoService;

        public AccesoController(IAccesoService accesoService)
        {
            _accesoService = accesoService;
        }

        [HttpPost("ValidarAbordaje")]
        public async Task<IActionResult> ValidarAbordaje([FromBody] AbordajeDto request)
        {
            if (request == null) return BadRequest(new { success = false, message = "Datos incompletos." });

            var result = await _accesoService.ValidarAbordajeAsync(request.ViajeId, request.UsuarioId);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}