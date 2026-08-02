using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.WebMVC.Services;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class ViajeController : Controller
    {
        private readonly ITransporteApiService _transporteApi;
        private readonly ICatalogoApiService _catalogoApi;

        public ViajeController(ITransporteApiService transporteApi, ICatalogoApiService catalogoApi)
        {
            _transporteApi = transporteApi;
            _catalogoApi = catalogoApi;
        }

        // 1. VISTA: INDEX
        public async Task<IActionResult> Index()
        {
            var lista = await _transporteApi.ObtenerViajesAsync();
            return View(lista);
        }

        // 2. VISTA: CREATE
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Create()
        {
            await CargarListasDesplegables();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Create(SaveViajeDto dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarListasDesplegables();
                return View(dto);
            }

            var exito = await _transporteApi.RegistrarViajeAsync(dto);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Se produjo un error al registrar el viaje mediante la API.");
            await CargarListasDesplegables();
            return View(dto);
        }

        // 3. VISTA: DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var viaje = await _transporteApi.ObtenerViajePorIdAsync(id);
            if (viaje == null) return NotFound();

            return View(viaje);
        }

        [Authorize(Roles = "AdminTransporte,Administrador,Conductor")]
        public async Task<IActionResult> Edit(int id)
        {
            var viaje = await _transporteApi.ObtenerViajePorIdAsync(id);
            if (viaje == null) return NotFound();

            await CargarListasDesplegables();
            return View(viaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador,Conductor")]
        public async Task<IActionResult> Edit(Viaje modelo)
        {
            ModelState.Remove("CreationDate");
            ModelState.Remove("Ruta");
            ModelState.Remove("Autobus");
            ModelState.Remove("Conductor");

            if (!ModelState.IsValid)
            {
                await CargarListasDesplegables();
                return View(modelo);
            }

            var exito = await _transporteApi.ActualizarViajeAsync(modelo);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al intentar actualizar el estado del viaje en la API.");
            await CargarListasDesplegables();
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var viaje = await _transporteApi.ObtenerViajePorIdAsync(id);
            if (viaje == null) return NotFound();

            return View(viaje);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exito = await _transporteApi.EliminarViajeAsync(id);

            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo anular el viaje mediante la API.");
            var viaje = await _transporteApi.ObtenerViajePorIdAsync(id);
            return View(viaje);
        }

        // --- MÉTODO PARA LLENAR LOS 4 DROPDOWNLISTS DESDE LA API ---
        private async Task CargarListasDesplegables()
        {
            var rutas = await _catalogoApi.ObtenerRutasAsync();
            var horarios = await _transporteApi.ObtenerHorariosAsync();

            // Consumo de los componentes físicos y de personal desde la API
            var buses = await _catalogoApi.ObtenerAutobusesAsync();
            var conductores = await _catalogoApi.ObtenerConductoresAsync();

            ViewBag.Rutas = new SelectList(rutas, "Id", "NombreRuta");
            ViewBag.Horarios = new SelectList(horarios, "Id", "HoraSalida");
            ViewBag.Autobuses = new SelectList(buses, "Id", "Placa");
            ViewBag.Conductores = new SelectList(conductores, "Id", "NombreCompleto");
        }
    }
}