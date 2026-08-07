using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoService _service;
        private readonly IAutobusRepository _autobusRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IHorarioRepository _horarioRepo;
        private readonly IRutaRepository _rutaRepo;
        private readonly IViajeRepository _viajeRepo;

        public CatalogoController(
            ICatalogoService service,
            IAutobusRepository autobusRepo,
            IUsuarioRepository usuarioRepo,
            IHorarioRepository horarioRepo,
            IRutaRepository rutaRepo,
            IViajeRepository viajeRepo)
        {
            _service = service;
            _autobusRepo = autobusRepo;
            _usuarioRepo = usuarioRepo;
            _horarioRepo = horarioRepo;
            _rutaRepo = rutaRepo;
            _viajeRepo = viajeRepo;
        }

        [HttpPost("autobus")]
        public async Task<IActionResult> RegistrarAutobus([FromBody] CreateAutobusDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevoAutobus = new Autobus
            {
                Placa = dto.Placa,
                CapacidadMaxima = dto.CapacidadMaxima,
                EstadoOperativo = (EstadoAutobus)dto.EstadoOperativo
            };

            return Ok(await _service.RegistrarAutobusAsync(nuevoAutobus));
        }

        [HttpPost("conductor")]
        public async Task<IActionResult> RegistrarConductor([FromBody] CreateConductorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevoConductor = new Usuario
            {
                IdentificacionInstitucional = dto.Identificacion,
                NombreCompleto = dto.Nombres,
                IsActive = dto.EstadoLaboral == 1,
                Rol = RolUsuario.Conductor
            };

            return Ok(await _service.RegistrarConductorAsync(nuevoConductor));
        }

        [HttpPost("ruta")]
        public async Task<IActionResult> RegistrarRuta([FromBody] CreateRutaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevaRuta = new Ruta
            {
                NombreRuta = dto.NombreRuta
            };

            return Ok(await _service.RegistrarRutaAsync(nuevaRuta));
        }

        [HttpPost("horario")]
        public async Task<IActionResult> RegistrarHorario([FromBody] CreateHorarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevoHorario = new Horario
            {
                RutaId = dto.RutaId,
                DiasOperacion = dto.DiasOperacion,
                HoraSalida = dto.HoraSalida
            };

            return Ok(await _service.RegistrarHorarioAsync(nuevoHorario));
        }

        [HttpGet("rutas")]
        public async Task<IActionResult> GetRutas() => Ok(await _service.ObtenerRutasAsync());

        [HttpGet("autobuses")]
        public async Task<IActionResult> GetAutobuses()
        {
            var result = await _autobusRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("conductores")]
        public async Task<IActionResult> GetConductores()
        {
            var result = await _usuarioRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("horarios")]
        public async Task<IActionResult> GetHorarios()
        {
            var result = await _horarioRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("horario")]
        public async Task<IActionResult> ActualizarHorario([FromBody] Horario horario)
        {
            if (horario == null || horario.Id == 0) return BadRequest(new { success = false, message = "ID requerido." });

            var originalResult = await _horarioRepo.GetByIdAsync(horario.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var horarioOriginal = (originalResult.Data as Horario)!;
            horarioOriginal.RutaId = horario.RutaId;
            horarioOriginal.DiasOperacion = horario.DiasOperacion;
            horarioOriginal.HoraSalida = horario.HoraSalida;

            var result = await _horarioRepo.UpdateEntityAsync(horarioOriginal);
            return Ok(result);
        }

        [HttpDelete("horario/{id}")]
        public async Task<IActionResult> EliminarHorario(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "ID inválido." });

            var originalResult = await _horarioRepo.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _horarioRepo.DeleteEntityAsync((originalResult.Data as Horario)!);
            return Ok(result);
        }

        [HttpPut("autobus")]
        public async Task<IActionResult> ActualizarAutobus([FromBody] Autobus autobus)
        {
            if (autobus == null || autobus.Id == 0) return BadRequest(new { success = false, message = "ID requerido." });

            var originalResult = await _autobusRepo.GetByIdAsync(autobus.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Autobús no encontrado." });

            var autobusOriginal = (originalResult.Data as Autobus)!;
            autobusOriginal.Placa = autobus.Placa;
            autobusOriginal.CapacidadMaxima = autobus.CapacidadMaxima;
            autobusOriginal.EstadoOperativo = autobus.EstadoOperativo;

            var result = await _autobusRepo.UpdateEntityAsync(autobusOriginal);
            return Ok(result);
        }

        [HttpDelete("autobus/{id}")]
        public async Task<IActionResult> EliminarAutobus(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "ID inválido." });

            var originalResult = await _autobusRepo.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Autobús no encontrado." });

            var result = await _autobusRepo.DeleteEntityAsync((originalResult.Data as Autobus)!);
            return Ok(result);
        }

        [HttpPut("ruta")]
        public async Task<IActionResult> ActualizarRuta([FromBody] Ruta ruta)
        {
            if (ruta == null || ruta.Id == 0) return BadRequest(new { success = false, message = "ID requerido." });

            var originalResult = await _rutaRepo.GetByIdAsync(ruta.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Ruta no encontrada." });

            var rutaOriginal = (originalResult.Data as Ruta)!;
            rutaOriginal.NombreRuta = ruta.NombreRuta;

            var result = await _rutaRepo.UpdateEntityAsync(rutaOriginal);
            return Ok(result);
        }

        [HttpDelete("ruta/{id}")]
        public async Task<IActionResult> EliminarRuta(int id)
        {
            if (id <= 0)
                return BadRequest(new { success = false, errorType = "ValidationError", message = "ID inválido." });

            var originalResult = await _rutaRepo.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null)
                return NotFound(new { success = false, errorType = "NotFound", message = "Ruta no encontrada." });

            bool tieneViajesActivos = await _viajeRepo.RutaTieneViajesActivosAsync(id);

            if (tieneViajesActivos)
            {
                return BadRequest(new
                {
                    success = false,
                    errorType = "DependencyConflict",
                    message = "No se puede eliminar la ruta: tiene viajes asociados programados o en curso."
                });
            }

            var result = await _rutaRepo.DeleteEntityAsync((originalResult.Data as Ruta)!);
            return Ok(result);
        }
    }
}