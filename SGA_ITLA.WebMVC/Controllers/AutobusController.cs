using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Application.Dtos.Catalogo;
using SGA_ITLA.Application.Interfaces.Catalogo;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Repositories;
using SGA_ITLA.WebMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class AutobusController : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IAutobusRepository _autobusRepository;
        private readonly IViajeRepository _viajeRepo; 

        public AutobusController(ICatalogoService catalogoService, IAutobusRepository autobusRepository, IViajeRepository viajeRepo)
        {
            _catalogoService = catalogoService;
            _autobusRepository = autobusRepository;
            _viajeRepo = viajeRepo;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _autobusRepository.GetAllAsync();
            var autobuses = result.Data as IEnumerable<Autobus> ?? new List<Autobus>();

            var viajesResult = await _viajeRepo.GetViajesDetalladosAsync();
            var viajes = viajesResult.Data as IEnumerable<Viaje> ?? new List<Viaje>();

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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Create(CreateAutobusDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var autobus = new Autobus
            {
                Placa = dto.Placa,
                CapacidadMaxima = dto.CapacidadMaxima,
                EstadoOperativo = (EstadoAutobus)dto.EstadoOperativo
            };

            var result = await _catalogoService.RegistrarAutobusAsync(autobus);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _autobusRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Autobus);
        }

        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _autobusRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Autobus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Edit(Autobus modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var originalResult = await _autobusRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var autobusOriginal = originalResult.Data as Autobus;
            autobusOriginal!.Placa = modelo.Placa;
            autobusOriginal.CapacidadMaxima = modelo.CapacidadMaxima;
            autobusOriginal.EstadoOperativo = modelo.EstadoOperativo;

            var result = await _autobusRepository.UpdateEntityAsync(autobusOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _autobusRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Autobus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _autobusRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _autobusRepository.DeleteEntityAsync((originalResult.Data as Autobus)!);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View((originalResult.Data as Autobus)!);
        }
    }
}