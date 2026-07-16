using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Interfaces.Transporte;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
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
        public async Task<IActionResult> ValidarAbordaje(int viajeId, int estudianteId)
        {
            var result = await _accesoService.ValidarAbordajeAsync(viajeId, estudianteId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}