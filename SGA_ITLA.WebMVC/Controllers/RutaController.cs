using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Catalogo;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class RutaController : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IRutaRepository _rutaRepository;

        // Inyección directa (In-Process)
        public RutaController(ICatalogoService catalogoService, IRutaRepository rutaRepository)
        {
            _catalogoService = catalogoService;
            _rutaRepository = rutaRepository;
        }

        // 1. VISTA: INDEX (Listado)
        public async Task<IActionResult> Index()
        {
            var result = await _rutaRepository.GetAllAsync();
            var lista = result.Data as IEnumerable<Ruta> ?? new List<Ruta>();
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

            var ruta = new Ruta
            {
                NombreRuta = dto.NombreRuta
            };

            // Cumpliendo el audio: llamamos al servicio, no a la API
            var result = await _catalogoService.RegistrarRutaAsync(ruta);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        // 3. VISTA: DETAILS (Todos pueden ver)
        public async Task<IActionResult> Details(int id)
        {
            var result = await _rutaRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Ruta);
        }

        // 4. VISTA: EDIT (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _rutaRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Ruta);
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

            var originalResult = await _rutaRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var rutaOriginal = originalResult.Data as Ruta;
            rutaOriginal.NombreRuta = modelo.NombreRuta;

            var result = await _rutaRepository.UpdateEntityAsync(rutaOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(modelo);
        }

        // 5. VISTA: DELETE (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _rutaRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Ruta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _rutaRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _rutaRepository.DeleteEntityAsync(originalResult.Data as Ruta);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(originalResult.Data as Ruta);
        }
    }
}