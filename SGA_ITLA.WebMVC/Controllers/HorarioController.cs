using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.WebMVC.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class HorarioController : Controller
    {
        private readonly ITransporteApiService _transporteApi;
        private readonly ICatalogoApiService _catalogoApi;

        public HorarioController(ITransporteApiService transporteApi, ICatalogoApiService catalogoApi)
        {
            _transporteApi = transporteApi;
            _catalogoApi = catalogoApi;
        }

        // 1. VISTA: INDEX
        public async Task<IActionResult> Index()
        {
            var horarios = await _transporteApi.ObtenerHorariosAsync();
            var rutas = await _catalogoApi.ObtenerRutasAsync();

            foreach (var h in horarios)
            {
                h.Ruta = rutas.FirstOrDefault(r => r.Id == h.RutaId);
            }

            return View(horarios);
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

            var exito = await _transporteApi.RegistrarHorarioAsync(dto);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Ocurrió un error al registrar el horario a través de la API.");
            await CargarRutasViewBag();
            return View(dto);
        }

        // 3. VISTA: DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var horario = await _transporteApi.ObtenerHorarioPorIdAsync(id);
            if (horario == null) return NotFound();

            // 🔥 SOLUCIÓN: Buscar y asignar la ruta para los detalles
            horario.Ruta = await _catalogoApi.ObtenerRutaPorIdAsync(horario.RutaId);

            return View(horario);
        }

        // 4. VISTA: EDIT (Solo Administradores)
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Edit(int id)
        {
            var horario = await _transporteApi.ObtenerHorarioPorIdAsync(id);
            if (horario == null) return NotFound();

            await CargarRutasViewBag();
            return View(horario);
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

            var exito = await _transporteApi.ActualizarHorarioAsync(modelo);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar el horario en la API.");
            await CargarRutasViewBag();
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var horario = await _transporteApi.ObtenerHorarioPorIdAsync(id);
            if (horario == null) return NotFound();

            horario.Ruta = await _catalogoApi.ObtenerRutaPorIdAsync(horario.RutaId);

            return View(horario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exito = await _transporteApi.EliminarHorarioAsync(id);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo eliminar el horario mediante la API.");
            var horario = await _transporteApi.ObtenerHorarioPorIdAsync(id);
            return View(horario);
        }

        private async Task CargarRutasViewBag()
        {
            var rutas = await _catalogoApi.ObtenerRutasAsync();
            ViewBag.Rutas = new SelectList(rutas, "Id", "NombreRuta");
        }
    }
}