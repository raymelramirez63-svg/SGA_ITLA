using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.WebMVC.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize(Roles = "AdminAutorizaciones,Administrador,Auditor")]
    public class AutorizacionController : Controller
    {
        private readonly IAutorizacionApiService _autorizacionApi;
        private readonly IUsuarioApiService _usuarioApi;

        public AutorizacionController(IAutorizacionApiService autorizacionApi, IUsuarioApiService usuarioApi)
        {
            _autorizacionApi = autorizacionApi;
            _usuarioApi = usuarioApi;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _autorizacionApi.ObtenerAutorizacionesAsync();
            return View(lista);
        }

        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Create()
        {
            await CargarUsuariosViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Create(EmitirTicketDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuariosViewBag();
                return View(dto);
            }

            var exito = await _autorizacionApi.EmitirTicketAsync(dto);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al emitir el ticket en la API.");
            await CargarUsuariosViewBag();
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var autorizaciones = await _autorizacionApi.ObtenerAutorizacionesAsync();
            var aut = autorizaciones.FirstOrDefault(a => a.Id == id);
            if (aut == null) return NotFound();
            return View(aut);
        }

        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var autorizaciones = await _autorizacionApi.ObtenerAutorizacionesAsync();
            var aut = autorizaciones.FirstOrDefault(a => a.Id == id);
            if (aut == null) return NotFound();

            await CargarUsuariosViewBag();
            return View(aut);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Edit(Autorizacion modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("Usuario");

            if (!ModelState.IsValid)
            {
                await CargarUsuariosViewBag();
                return View(modelo);
            }

            var exito = await _autorizacionApi.ActualizarAutorizacionAsync(modelo);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar la autorización.");
            await CargarUsuariosViewBag();
            return View(modelo);
        }

        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var autorizaciones = await _autorizacionApi.ObtenerAutorizacionesAsync();
            var aut = autorizaciones.FirstOrDefault(a => a.Id == id);
            if (aut == null) return NotFound();
            return View(aut);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exito = await _autorizacionApi.EliminarAutorizacionAsync(id);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo anular la autorización.");
            var autorizaciones = await _autorizacionApi.ObtenerAutorizacionesAsync();
            return View(autorizaciones.FirstOrDefault(a => a.Id == id));
        }

        [HttpGet]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public IActionResult Recargar() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminAutorizaciones,Administrador")]
        public async Task<IActionResult> Recargar(RecargarTarjetaDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var exito = await _autorizacionApi.RecargarTarjetaAsync(dto);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo procesar la recarga en la API. Verifique los datos.");
            return View(dto);
        }

        private async Task CargarUsuariosViewBag()
        {
            var usuarios = await _usuarioApi.ObtenerUsuariosAsync();
            var beneficiarios = usuarios.Where(u => u.Rol == RolUsuario.Estudiante || u.Rol == RolUsuario.Empleado);
            ViewBag.Usuarios = new SelectList(beneficiarios, "Id", "NombreCompleto");
        }
    }
}