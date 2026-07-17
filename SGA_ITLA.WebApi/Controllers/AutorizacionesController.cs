using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionesController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public AutorizacionesController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }

        [HttpPost("EmitirTicket")]
        public async Task<IActionResult> EmitirTicket([FromBody] EmitirTicketDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _autorizacionService.EmitirTicketMensualAsync(
                request.UsuarioId,
                request.PagoId,
                request.FechaInicio);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("RecargarTarjeta")]
        public async Task<IActionResult> RecargarTarjeta([FromBody] RecargarTarjetaDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _autorizacionService.RecargarTarjetaAsync(request.UsuarioId, request.Monto);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}