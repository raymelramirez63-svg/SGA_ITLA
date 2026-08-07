using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.WebMVC.Services;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class SolicitudController : Controller
    {
        private readonly ISolicitudApiService _solicitudApi;

        public SolicitudController(ISolicitudApiService solicitudApi)
        {
            _solicitudApi = solicitudApi;
        }

        [HttpGet]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Index()
        {
            var lista = await _solicitudApi.ObtenerMisSolicitudesAsync();
            return View(lista);
        }

        [HttpGet]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Pendientes()
        {
            var lista = await _solicitudApi.ObtenerPendientesAsync();
            return View(lista);
        }

        [HttpGet]
        [Authorize(Roles = "Estudiante")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Create(TipoAutorizacion tipoSolicitado, string? comentario)
        {
            var exito = await _solicitudApi.CrearSolicitudAsync(tipoSolicitado, comentario);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Ocurrió un error o ya tienes una solicitud de ticket pendiente.");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Aprobar(int id, int pagoId, decimal? monto)
        {
            await _solicitudApi.AprobarSolicitudAsync(id, pagoId, monto);
            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Rechazar(int id, string motivo)
        {
            await _solicitudApi.RechazarSolicitudAsync(id, motivo);
            return RedirectToAction(nameof(Pendientes));
        }
    }
}