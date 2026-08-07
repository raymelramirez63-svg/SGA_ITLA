using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.WebMVC.Models;
using SGA_ITLA.WebMVC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class AutobusController : Controller
    {
        private readonly ICatalogoApiService _catalogoApi;
        private readonly ITransporteApiService _transporteApi;

        // 🔥 Inyectamos solo servicios HTTP, respetando el diseño orientado a servicios
        public AutobusController(ICatalogoApiService catalogoApi, ITransporteApiService transporteApi)
        {
            _catalogoApi = catalogoApi;
            _transporteApi = transporteApi;
        }

        public async Task<IActionResult> Index()
        {
            var autobuses = await _catalogoApi.ObtenerAutobusesAsync();
            var viajes = await _transporteApi.ObtenerViajesAsync();

            var lista = autobuses.Select(a => new AutobusListItemVM
            {
                Id = a.Id,
                Placa = a.Placa,
                CapacidadMaxima = a.CapacidadMaxima,
                EstadoOperativo = a.EstadoOperativo,
                ChoferAsignado = viajes
                    .Where(v => v.AutobusId == a.Id &&
                                (v.Estado == EstadoViaje.Programado || v.Estado == EstadoViaje.EnCurso))
                    .OrderByDescending(v => v.HorarioSalidaPlanificada)
                    .Select(v => v.Conductor != null ? v.Conductor.NombreCompleto : $"Conductor #{v.ConductorId}")
                    .FirstOrDefault()
            }).ToList();

            return View(lista);
        }

        [Authorize(Roles = "AdminTransporte")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Create(CreateAutobusDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            // Enviamos el DTO a la API para creación
            var exito = await _catalogoApi.RegistrarAutobusAsync(dto);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al registrar el autobús en la API.");
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var autobuses = await _catalogoApi.ObtenerAutobusesAsync();
            var bus = autobuses.FirstOrDefault(a => a.Id == id);
            if (bus == null) return NotFound();
            return View(bus);
        }

        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Edit(int id)
        {
            var autobuses = await _catalogoApi.ObtenerAutobusesAsync();
            var bus = autobuses.FirstOrDefault(a => a.Id == id);
            if (bus == null) return NotFound();
            return View(bus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Edit(Autobus modelo)
        {
            ModelState.Remove("CreationDate");
            if (!ModelState.IsValid) return View(modelo);

            var exito = await _catalogoApi.ActualizarAutobusAsync(modelo);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Error al actualizar el autobús en la API.");
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Delete(int id)
        {
            var autobuses = await _catalogoApi.ObtenerAutobusesAsync();
            var bus = autobuses.FirstOrDefault(a => a.Id == id);
            if (bus == null) return NotFound();
            return View(bus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exito = await _catalogoApi.EliminarAutobusAsync(id);
            if (exito) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "No se pudo eliminar el autobús mediante la API.");
            var autobuses = await _catalogoApi.ObtenerAutobusesAsync();
            return View(autobuses.FirstOrDefault(a => a.Id == id));
        }
    }
}