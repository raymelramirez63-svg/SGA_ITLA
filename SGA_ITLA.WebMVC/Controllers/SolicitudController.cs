using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class SolicitudController : Controller
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

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Index()
        {
            var usuarioId = await ObtenerUsuarioIdActualAsync();
            var lista = await _solicitudRepo.ObtenerPorUsuarioAsync(usuarioId);
            return View(lista);
        }

        [Authorize(Roles = "Estudiante")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Create(TipoAutorizacion tipoSolicitado, string? comentario)
        {
            var usuarioId = await ObtenerUsuarioIdActualAsync();
            var result = await _solicitudService.CrearSolicitudAsync(usuarioId, tipoSolicitado, comentario);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View();
        }

        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Pendientes()
        {
            var lista = await _solicitudRepo.ObtenerPendientesAsync();
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Aprobar(int id, int pagoId, decimal? monto)
        {
            var result = await _solicitudService.AprobarSolicitudAsync(id, pagoId, monto);
            if (!result.Success) TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Rechazar(int id, string motivo)
        {
            var result = await _solicitudService.RechazarSolicitudAsync(id, motivo);
            if (!result.Success) TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Pendientes));
        }

        private async Task<int> ObtenerUsuarioIdActualAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email)) return 0;

            var usuario = await _usuarioRepo.GetByEmailAsync(email);
            return usuario?.Id ?? 0;
        }
    }
}