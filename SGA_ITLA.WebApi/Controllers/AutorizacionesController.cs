using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
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
        public async Task<IActionResult> EmitirTicket(int estudianteId)
        {
            var result = await _autorizacionService.EmitirTicketMensualAsync(estudianteId);
            return Ok(result);
        }

        [HttpPost("RecargarTarjeta")]
        public async Task<IActionResult> RecargarTarjeta(int estudianteId, decimal monto)
        {
            var result = await _autorizacionService.RecargarTarjetaAsync(estudianteId, monto);
            return Ok(result);
        }
    }
}