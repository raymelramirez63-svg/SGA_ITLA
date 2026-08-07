using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Interfaces;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacionesController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAutorizacionRepository _autorizacionRepo; 

        public AutorizacionesController(
            IAutorizacionService autorizacionService,
            IUsuarioRepository usuarioRepository,
            IAutorizacionRepository autorizacionRepo)
        {
            _autorizacionService = autorizacionService;
            _usuarioRepository = usuarioRepository;
            _autorizacionRepo = autorizacionRepo;
        }

        [HttpGet]
        [Authorize(Roles = "AdminAutorizaciones,Administrador,Auditor")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _autorizacionRepo.GetAllAsync());
        }

        [HttpPut]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Update([FromBody] Autorizacion autorizacion)
        {
            return Ok(await _autorizacionRepo.UpdateEntityAsync(autorizacion));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _autorizacionRepo.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return Ok(await _autorizacionRepo.DeleteEntityAsync((Autorizacion)result.Data));
        }

        [HttpPost("EmitirTicket")]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
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
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> RecargarTarjeta([FromBody] RecargarTarjetaDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _usuarioRepository.ObtenerPorIdentificacionAsync(request.IdentificacionInstitucional);

            if (usuario == null)
            {
                return BadRequest(new { success = false, message = $"Error: No existe ningún usuario activo registrado con la identificación '{request.IdentificacionInstitucional}'." });
            }

            var result = await _autorizacionService.RecargarTarjetaAsync(usuario.Id, request.Monto);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}