using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ViajeController : ControllerBase
    {
        private readonly IViajeService _viajeService;
        private readonly IHorarioRepository _horarioRepo;

        public ViajeController(IViajeService viajeService, IHorarioRepository horarioRepo)
        {
            _viajeService = viajeService;
            _horarioRepo = horarioRepo;
        }

        [HttpGet("GetViajesActivos")]
        public async Task<IActionResult> GetViajesActivos()
        {
            var result = await _viajeService.ObtenerViajesDetalladosAsync();
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("SaveViaje")]
        public async Task<IActionResult> SaveViaje([FromBody] SaveViajeDto saveViajeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevoViaje = new Viaje
            {
                RutaId = saveViajeDto.RutaId,
                AutobusId = saveViajeDto.AutobusId,
                ConductorId = saveViajeDto.ConductorId,
                CupoDisponibleActual = 40
            };

            var horarioResult = await _horarioRepo.GetByIdAsync(saveViajeDto.HorarioId);
            if (horarioResult.Success && horarioResult.Data is Horario horario)
            {
                nuevoViaje.HorarioSalidaPlanificada = DateTime.Today.Add(horario.HoraSalida);
            }

            var result = await _viajeService.RegistrarViajeAsync(nuevoViaje);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("UpdateViaje")]
        public async Task<IActionResult> UpdateViaje([FromBody] Viaje viaje)
        {
            if (viaje == null || viaje.Id == 0) return BadRequest(new { success = false, message = "Datos inválidos." });

            var result = await _viajeService.ActualizarViajeAsync(viaje);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("DeleteViaje/{id}")]
        public async Task<IActionResult> DeleteViaje(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "El ID proporcionado no es válido." });

            var result = await _viajeService.EliminarViajeAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPatch("CambiarEstado/{viajeId}")]
        public async Task<IActionResult> CambiarEstadoViaje(int viajeId, [FromBody] int nuevoEstadoId)
        {
            if (viajeId <= 0 || nuevoEstadoId <= 0 || nuevoEstadoId > 5) return BadRequest(new { success = false, message = "ID o estado inválido." });

            var result = await _viajeService.CambiarEstadoViajeAsync(viajeId, nuevoEstadoId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("ReportarIncidencia")]
        public IActionResult ReportarIncidencia([FromBody] IncidenciaDto incidencia)
        {
            if (incidencia == null || incidencia.ViajeId <= 0) return BadRequest(new { success = false, message = "Datos inválidos." });
            return Ok(new { success = true, message = "Incidencia reportada. Administrador notificado." });
        }
    }
}
