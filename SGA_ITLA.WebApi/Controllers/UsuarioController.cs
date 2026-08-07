using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize(Roles = "AdminTransporte,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IViajeRepository _viajeRepo;

        public UsuarioController(IUsuarioRepository usuarioRepo, IViajeRepository viajeRepo)
        {
            _usuarioRepo = usuarioRepo;
            _viajeRepo = viajeRepo;
        }

        [HttpGet]
        [Authorize(Roles = "AdminTransporte,Administrador,AdminAutorizaciones")]
        public async Task<IActionResult> GetAll() => Ok(await _usuarioRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _usuarioRepo.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            return Ok(await _usuarioRepo.SaveEntityAsync(usuario));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Usuario usuario)
        {
            return Ok(await _usuarioRepo.UpdateEntityAsync(usuario));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioRepo.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound(result);

            var usuario = (Usuario)result.Data;

            if (usuario.Rol == RolUsuario.Conductor)
            {
                bool asignado = await _viajeRepo.ConductorTieneViajesActivosGlobalAsync(id);
                if (asignado) return BadRequest(new { success = false, message = "No se puede suspender este conductor: tiene viajes programados o en curso." });
            }

            return Ok(await _usuarioRepo.DeleteEntityAsync(usuario));
        }
    }
}