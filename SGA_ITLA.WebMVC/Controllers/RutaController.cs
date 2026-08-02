using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.WebMVC.Services; // Usamos nuestro nuevo servicio API

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class RutaController : Controller
    {
        private readonly ICatalogoApiService _apiService;

        // Inyección del servicio HTTP (Cumpliendo los lineamientos)
        public RutaController(ICatalogoApiService apiService)
        {
            _apiService = apiService;
        }

        // 1. VISTA: INDEX (Listado)
        public async Task<IActionResult> Index()
        {
            var lista = await _apiService.ObtenerRutasAsync();
            return View(lista);
        }

        // 2. VISTA: CREATE (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Create(CreateRutaDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var exito = await _apiService.RegistrarRutaAsync(dto);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Hubo un problema al registrar la ruta en la API.");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ruta = await _apiService.ObtenerRutaPorIdAsync(id);
            if (ruta == null) return NotFound();

            return View(ruta);
        }

        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var ruta = await _apiService.ObtenerRutaPorIdAsync(id);
            if (ruta == null) return NotFound();

            return View(ruta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(Ruta modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("Paradas");
            ModelState.Remove("Horarios");

            if (!ModelState.IsValid) return View(modelo);

            var exito = await _apiService.ActualizarRutaAsync(modelo);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo actualizar la ruta en la API.");
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var ruta = await _apiService.ObtenerRutaPorIdAsync(id);
            if (ruta == null) return NotFound();

            return View(ruta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exito = await _apiService.EliminarRutaAsync(id);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Ocurrió un error al intentar eliminar la ruta mediante la API.");

            var ruta = await _apiService.ObtenerRutaPorIdAsync(id);
            return View(ruta);
        }
    }
}