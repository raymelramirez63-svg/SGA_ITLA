using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Auditoria;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize(Roles = "Auditor,Administrador,AdminTransporte")]
    public class AuditoriaController : Controller
    {
        private readonly IAuditoriaRepository _auditoriaRepo;

        public AuditoriaController(IAuditoriaRepository auditoriaRepo)
        {
            _auditoriaRepo = auditoriaRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _auditoriaRepo.GetAllAsync();
            var lista = result.Data as IEnumerable<RegistroAuditoria> ?? new List<RegistroAuditoria>();

            var historialOrdenado = lista.OrderByDescending(x => x.CreationDate).ToList();

            return View(historialOrdenado);
        }
    }
}