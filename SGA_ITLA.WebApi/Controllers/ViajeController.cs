using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ViajeController : ControllerBase
    {
        private readonly IViajeService _viajeService;

        public ViajeController(IViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("GetViajesActivos")]
        public async Task<IActionResult> GetViajesActivos()
        {
            var result = await _viajeService.GetAllViajesActivosAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("SaveViaje")]
        public async Task<IActionResult> SaveViaje([FromBody] SaveViajeDto saveViajeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _viajeService.SaveViajeAsync(saveViajeDto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}