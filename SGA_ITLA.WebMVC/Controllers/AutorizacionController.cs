using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Application.Interfaces.Autorizaciones;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Autorizaciones;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Application.Dtos.Autorizaciones;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.WebMVC.Controllers
{
    //  solo los administradores puedan entrar
    [Authorize(Roles = "AdminAutorizaciones,Administrador")]
    public class AutorizacionController : Controller
    {
        private readonly IAutorizacionService _autorizacionService;
        private readonly IAutorizacionRepository _autorizacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        // Inyección In-Process (Cero Web API)
        public AutorizacionController(
            IAutorizacionService autorizacionService,
            IAutorizacionRepository autorizacionRepository,
            IUsuarioRepository usuarioRepository)
        {
            _autorizacionService = autorizacionService;
            _autorizacionRepository = autorizacionRepository;
            _usuarioRepository = usuarioRepository;
        }

        // 1. VISTA: INDEX (Listado de Autorizaciones)
        public async Task<IActionResult> Index()
        {
            var result = await _autorizacionRepository.GetAllAsync();
            var lista = result.Data as IEnumerable<Autorizacion> ?? new List<Autorizacion>();
            return View(lista);
        }

        // 2. VISTA: CREATE (Emitir Ticket Mensual)
        public async Task<IActionResult> Create()
        {
            await CargarUsuariosViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmitirTicketDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarUsuariosViewBag();
                return View(dto);
            }

            // Llamada directa al servicio de Aplicación 
            var result = await _autorizacionService.EmitirTicketMensualAsync(dto.UsuarioId, dto.PagoId, dto.FechaInicio);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarUsuariosViewBag();
            return View(dto);
        }

        // 3. VISTA: DETAILS (Ver Estado de la Tarjeta/Ticket)
        public async Task<IActionResult> Details(int id)
        {
            var result = await _autorizacionRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Autorizacion);
        }

        // 4. VISTA: EDIT (Modificar saldo o estado activo)
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _autorizacionRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            await CargarUsuariosViewBag();
            return View(result.Data as Autorizacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Autorizacion modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("Usuario");

            if (!ModelState.IsValid)
            {
                await CargarUsuariosViewBag();
                return View(modelo);
            }

            var originalResult = await _autorizacionRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var autorizacionOriginal = originalResult.Data as Autorizacion;

            autorizacionOriginal!.Tipo = modelo.Tipo;
            autorizacionOriginal.SaldoDisponible = modelo.SaldoDisponible;
            autorizacionOriginal.FechaFinVigencia = modelo.FechaFinVigencia;
            autorizacionOriginal.IsActive = modelo.IsActive;

            var result = await _autorizacionRepository.UpdateEntityAsync(autorizacionOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarUsuariosViewBag();
            return View(modelo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _autorizacionRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Autorizacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _autorizacionRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _autorizacionRepository.DeleteEntityAsync((originalResult.Data as Autorizacion)!);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View((originalResult.Data as Autorizacion)!);
        }

        private async Task CargarUsuariosViewBag()
        {
            var result = await _usuarioRepository.GetAllAsync();
            var usuarios = result.Data as IEnumerable<Usuario> ?? new List<Usuario>();
            var beneficiarios = usuarios.Where(u => u.Rol == RolUsuario.Estudiante || u.Rol == RolUsuario.Empleado);
            ViewBag.Usuarios = new SelectList(beneficiarios, "Id", "NombreCompleto");
        }
    }
}