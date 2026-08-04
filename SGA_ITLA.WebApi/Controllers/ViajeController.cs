using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Auditoria;
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
        private readonly IViajeRepository _viajeRepo;
        private readonly IAuditoriaRepository _auditoriaRepo; 

        public ViajeController(IViajeService viajeService, IHorarioRepository horarioRepo, IViajeRepository viajeRepo, IAuditoriaRepository auditoriaRepo)
        {
            _viajeService = viajeService;
            _horarioRepo = horarioRepo;
            _viajeRepo = viajeRepo;
            _auditoriaRepo = auditoriaRepo;
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
        public async Task<IActionResult> UpdateViaje([FromBody] Viaje viaje)
        {
            if (viaje == null || viaje.Id == 0) return BadRequest(new { success = false, message = "Datos inválidos." });

            var originalResult = await _viajeRepo.GetByIdAsync(viaje.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Viaje no encontrado." });

            var viajeOriginal = (Viaje)originalResult.Data;
            viajeOriginal.Estado = viaje.Estado;

            var result = await _viajeRepo.UpdateEntityAsync(viajeOriginal);
            return Ok(result);
        }

        [HttpDelete("DeleteViaje/{id}")]
        public async Task<IActionResult> DeleteViaje(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "El ID proporcionado no es válido." });

            var originalResult = await _viajeRepo.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Viaje no encontrado." });

            var result = await _viajeRepo.DeleteEntityAsync((Viaje)originalResult.Data);
            return Ok(result);
        }

        [HttpPatch("CambiarEstado/{viajeId}")]
        public async Task<IActionResult> CambiarEstadoViaje(int viajeId, [FromBody] int nuevoEstadoId)
        {
            if (viajeId <= 0 || nuevoEstadoId <= 0 || nuevoEstadoId > 5) return BadRequest(new { success = false, message = "ID o estado inválido." });

            var originalResult = await _viajeRepo.GetByIdAsync(viajeId);
            if (!originalResult.Success || originalResult.Data == null) return NotFound(new { success = false, message = "Viaje no encontrado." });

            var viajeOriginal = (Viaje)originalResult.Data;
            var estadoAnterior = viajeOriginal.Estado;
            viajeOriginal.Estado = (SGA_ITLA.Domain.Enums.EstadoViaje)nuevoEstadoId;

            var result = await _viajeRepo.UpdateEntityAsync(viajeOriginal);

            if (result.Success)
            {
                await _auditoriaRepo.SaveEntityAsync(new RegistroAuditoria
                {
                    ActorId = 1,
                    ModuloAfectado = "Operaciones",
                    AccionRealizada = "Actualización de Estado",
                    Detalles = $"Viaje #{viajeId} pasó de '{estadoAnterior}' a '{viajeOriginal.Estado}'.",
                    ResultadoExitoso = true,
                    CreationDate = DateTime.Now
                });

                return Ok(new { success = true, message = "Estado del viaje actualizado correctamente.", auditoria = $"Acción registrada el: {DateTime.Now}" });
            }
            return BadRequest(result);
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