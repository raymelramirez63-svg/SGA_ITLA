using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Infraestructure.Context;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionesController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;
        private readonly SgaContext _context;

        public AutorizacionesController(IAutorizacionService autorizacionService, SgaContext context)
        {
            _autorizacionService = autorizacionService;
            _context = context;
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

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdentificacionInstitucional == request.IdentificacionInstitucional);

            if (usuario == null)
            {
                return BadRequest(new { success = false, message = $"Error: No existe ningún usuario registrado con la identificación '{request.IdentificacionInstitucional}'." });
            }

            var result = await _autorizacionService.RecargarTarjetaAsync(usuario.Id, request.Monto);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}