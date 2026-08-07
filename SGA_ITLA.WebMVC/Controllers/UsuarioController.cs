using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.WebMVC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize(Roles = "AdminTransporte")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioApiService _usuarioApi;

        // Inyectamos el servicio HTTP, respetando los lineamientos
        public UsuarioController(IUsuarioApiService usuarioApi)
        {
            _usuarioApi = usuarioApi;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _usuarioApi.ObtenerUsuariosAsync();
            return View(lista);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid) return View(modelo);

            var exito = await _usuarioApi.RegistrarUsuarioAsync(modelo);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al intentar registrar el usuario a través de la API.");
            return View(modelo);
        }

        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _usuarioApi.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioApi.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario modelo)
        {
            ModelState.Remove("CreationDate");
            if (!ModelState.IsValid) return View(modelo);

            var usuarioOriginal = await _usuarioApi.ObtenerUsuarioPorIdAsync(modelo.Id);
            if (usuarioOriginal == null) return NotFound();

            usuarioOriginal.NombreCompleto = modelo.NombreCompleto;
            usuarioOriginal.IdentificacionInstitucional = modelo.IdentificacionInstitucional;
            usuarioOriginal.Email = modelo.Email;
            usuarioOriginal.Rol = modelo.Rol;

            if (!string.IsNullOrEmpty(modelo.PasswordHash))
            {
                usuarioOriginal.PasswordHash = modelo.PasswordHash;
            }

            var exito = await _usuarioApi.ActualizarUsuarioAsync(usuarioOriginal);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al intentar actualizar la cuenta en la API.");
            return View(modelo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioApi.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mensajeRespuesta = await _usuarioApi.EliminarUsuarioAsync(id);

            if (mensajeRespuesta == "OK") return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", mensajeRespuesta);
            var usuario = await _usuarioApi.ObtenerUsuarioPorIdAsync(id);
            return View(usuario);
        }
    }
}