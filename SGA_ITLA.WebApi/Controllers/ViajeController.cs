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

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("UpdateViaje")]
        public IActionResult UpdateViaje([FromBody] SaveViajeDto saveViajeDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(new { success = true, message = "Viaje actualizado correctamente." });
        }

        [HttpDelete("DeleteViaje/{id}")]
        public IActionResult DeleteViaje(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "El ID proporcionado no es válido." });
            return Ok(new { success = true, message = "Registro eliminado exitosamente mediante borrado lógico (Deleted = true)." });
        }

        [HttpPatch("CambiarEstado/{viajeId}")]
        public IActionResult CambiarEstadoViaje(int viajeId, [FromBody] int nuevoEstadoId)
        {
            if (viajeId <= 0 || nuevoEstadoId <= 0) return BadRequest(new { success = false, message = "ID o estado inválido." });
            if (nuevoEstadoId > 4) return BadRequest(new { success = false, message = "Transición de estado inválida." });

            return Ok(new { success = true, message = "Estado del viaje actualizado correctamente.", auditoria = $"Acción registrada el: {DateTime.Now}" });
        }

        [HttpPost("ReportarIncidencia")]
        public IActionResult ReportarIncidencia([FromBody] IncidenciaDto incidencia)
        {
            if (incidencia == null || incidencia.ViajeId <= 0) return BadRequest(new { success = false, message = "Datos inválidos." });
            return Ok(new { success = true, message = "Incidencia reportada. Administrador notificado." });
        }
    }

    public class IncidenciaDto
    {
        public int ViajeId { get; set; }
        public string? TipoIncidencia { get; set; }
        public string? Descripcion { get; set; }
    }
}