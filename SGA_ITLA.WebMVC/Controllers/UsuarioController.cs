using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _usuarioRepository.GetAllAsync();
            var lista = result.Data as IEnumerable<Usuario> ?? new List<Usuario>();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid) return View(modelo);

            var result = await _usuarioRepository.SaveEntityAsync(modelo);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(modelo);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _usuarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Usuario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _usuarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario modelo)
        {
            ModelState.Remove("CreationDate");

            if (!ModelState.IsValid) return View(modelo);

            var originalResult = await _usuarioRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var usuarioOriginal = originalResult.Data as Usuario;
            usuarioOriginal!.NombreCompleto = modelo.NombreCompleto;
            usuarioOriginal.IdentificacionInstitucional = modelo.IdentificacionInstitucional;
            usuarioOriginal.Email = modelo.Email;
            usuarioOriginal.Rol = modelo.Rol;

            if (!string.IsNullOrEmpty(modelo.PasswordHash))
            {
                usuarioOriginal.PasswordHash = modelo.PasswordHash;
            }

            var result = await _usuarioRepository.UpdateEntityAsync(usuarioOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(modelo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _usuarioRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _usuarioRepository.DeleteEntityAsync((originalResult.Data as Usuario)!);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View((originalResult.Data as Usuario)!);
        }
    }
}