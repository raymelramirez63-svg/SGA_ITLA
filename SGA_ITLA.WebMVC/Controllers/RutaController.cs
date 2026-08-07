using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.WebMVC.Services;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class RutaController : Controller
    {
        private readonly ICatalogoApiService _apiService;

        public RutaController(ICatalogoApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _apiService.ObtenerRutasAsync();
            return View(lista);
        }

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
            var mensajeRespuesta = await _apiService.EliminarRutaAsync(id);

            if (mensajeRespuesta == "OK") return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", mensajeRespuesta);

            var ruta = await _apiService.ObtenerRutaPorIdAsync(id);
            return View(ruta);
        }
    }
}