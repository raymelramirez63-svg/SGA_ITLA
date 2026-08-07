using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ISolicitudAutorizacionRepository _solicitudRepo;
        private readonly IUsuarioRepository _usuarioRepo;

        public SolicitudController(ISolicitudService solicitudService, ISolicitudAutorizacionRepository solicitudRepo, IUsuarioRepository usuarioRepo)
        {
            _solicitudService = solicitudService;
            _solicitudRepo = solicitudRepo;
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet("MisSolicitudes")]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> MisSolicitudes()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var usuario = await _usuarioRepo.GetByEmailAsync(email);
            if (usuario == null) return Unauthorized();

            var data = await _solicitudRepo.ObtenerPorUsuarioAsync(usuario.Id);
            return Ok(new OperationResult { Success = true, Data = data });
        }

        [HttpGet("Pendientes")]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Pendientes()
        {
            var data = await _solicitudRepo.ObtenerPendientesAsync();
            return Ok(new OperationResult { Success = true, Data = data });
        }

        [HttpPost("Crear")]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Crear([FromBody] CrearSolicitudDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email");
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var usuario = await _usuarioRepo.GetByEmailAsync(email);
            if (usuario == null) return Unauthorized();

            var result = await _solicitudService.CrearSolicitudAsync(usuario.Id, dto.TipoSolicitado, dto.Comentario);
            return Ok(result);
        }

        [HttpPost("Aprobar/{id}")]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Aprobar(int id, [FromBody] AprobarSolicitudDto dto)
        {
            return Ok(await _solicitudService.AprobarSolicitudAsync(id, dto.PagoId, dto.Monto));
        }

        [HttpPost("Rechazar/{id}")]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Rechazar(int id, [FromBody] RechazarSolicitudDto dto)
        {
            return Ok(await _solicitudService.RechazarSolicitudAsync(id, dto.Motivo));
        }
    }

    public class CrearSolicitudDto
    {
        public TipoAutorizacion TipoSolicitado { get; set; }
        public string? Comentario { get; set; }
    }
    public class AprobarSolicitudDto
    {
        public int PagoId { get; set; }
        public decimal? Monto { get; set; }
    }
    public class RechazarSolicitudDto
    {
        public string Motivo { get; set; } = string.Empty;
    }
}