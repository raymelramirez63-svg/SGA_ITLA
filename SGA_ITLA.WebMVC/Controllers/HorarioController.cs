using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Catalogo;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class HorarioController : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IHorarioRepository _horarioRepository;
        private readonly IRutaRepository _rutaRepository;

        public HorarioController(ICatalogoService catalogoService, IHorarioRepository horarioRepository, IRutaRepository rutaRepository)
        {
            _catalogoService = catalogoService;
            _horarioRepository = horarioRepository;
            _rutaRepository = rutaRepository;
        }

        // 1. VISTA: INDEX
        public async Task<IActionResult> Index()
        {
            var result = await _horarioRepository.GetAllAsync();
            var lista = result.Data as IEnumerable<Horario> ?? new List<Horario>();
            return View(lista);
        }

        // 2. VISTA: CREATE (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Create()
        {
            await CargarRutasViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Create(CreateHorarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarRutasViewBag();
                return View(dto);
            }

            var horario = new Horario
            {
                RutaId = dto.RutaId,
                DiasOperacion = dto.DiasOperacion,
                HoraSalida = dto.HoraSalida
            };

            var result = await _catalogoService.RegistrarHorarioAsync(horario);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarRutasViewBag();
            return View(dto);
        }

        // 3. VISTA: DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var result = await _horarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Horario);
        }

        // 4. VISTA: EDIT (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _horarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            await CargarRutasViewBag();
            return View(result.Data as Horario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(Horario modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("Ruta");

            if (!ModelState.IsValid)
            {
                await CargarRutasViewBag();
                return View(modelo);
            }

            var originalResult = await _horarioRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var horarioOriginal = originalResult.Data as Horario;
            horarioOriginal.RutaId = modelo.RutaId;
            horarioOriginal.DiasOperacion = modelo.DiasOperacion;
            horarioOriginal.HoraSalida = modelo.HoraSalida;

            var result = await _horarioRepository.UpdateEntityAsync(horarioOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarRutasViewBag();
            return View(modelo);
        }

        // 5. VISTA: DELETE (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _horarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Horario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _horarioRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _horarioRepository.DeleteEntityAsync(originalResult.Data as Horario);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(originalResult.Data as Horario);
        }

        private async Task CargarRutasViewBag()
        {
            var result = await _rutaRepository.GetAllAsync();
            var rutas = result.Data as IEnumerable<Ruta> ?? new List<Ruta>();
            ViewBag.Rutas = new SelectList(rutas, "Id", "NombreRuta");
        }
    }
}