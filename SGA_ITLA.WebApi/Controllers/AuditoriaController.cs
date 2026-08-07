using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Interfaces;
using System.Threading.Tasks;

namespace SGA_ITLA.WebApi.Controllers
{
    [Authorize(Roles = "AdminTransporte,Administrador,Auditor")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaRepository _auditoriaRepo;

        public AuditoriaController(IAuditoriaRepository auditoriaRepo)
        {
            _auditoriaRepo = auditoriaRepo;
        }

        [HttpGet("GetHistorial")]
        public async Task<IActionResult> GetHistorial()
        {
            var result = await _auditoriaRepo.GetAllAsync();
            return Ok(result);
        }
    }
}