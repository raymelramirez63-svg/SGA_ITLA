using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SGA_ITLA.Application.Interfaces.Transporte;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Domain.Entities.Transporte;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Application.Dtos.Transporte.Viajes;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize]
    public class ViajeController : Controller
    {
        private readonly IViajeService _viajeService;
        private readonly IViajeRepository _viajeRepository;
        private readonly IRutaRepository _rutaRepo;
        private readonly IAutobusRepository _autobusRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IHorarioRepository _horarioRepo;

        // los Selects
        public ViajeController(
            IViajeService viajeService,
            IViajeRepository viajeRepository,
            IRutaRepository rutaRepo,
            IAutobusRepository autobusRepo,
            IUsuarioRepository usuarioRepo,
            IHorarioRepository horarioRepo)
        {
            _viajeService = viajeService;
            _viajeRepository = viajeRepository;
            _rutaRepo = rutaRepo;
            _autobusRepo = autobusRepo;
            _usuarioRepo = usuarioRepo;
            _horarioRepo = horarioRepo;
        }

        // 1. VISTA: INDEX
        public async Task<IActionResult> Index()
        {
            var result = await _viajeRepository.GetViajesDetalladosAsync();
            var lista = result.Data as IEnumerable<Viaje> ?? new List<Viaje>();
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

            var viaje = new Viaje
            {
                RutaId = dto.RutaId,
                AutobusId = dto.AutobusId,
                ConductorId = dto.ConductorId,
                Estado = EstadoViaje.Programado,
                CupoDisponibleActual = 40 // Cupo inicial por defecto
            };

            var horarioResult = await _horarioRepo.GetByIdAsync(dto.HorarioId);
            if (horarioResult.Success && horarioResult.Data is Horario horario)
            {
                viaje.HorarioSalidaPlanificada = DateTime.Today.Add(horario.HoraSalida);
            }

            var result = await _viajeService.RegistrarViajeAsync(viaje);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarListasDesplegables();
            return View(dto);
        }

        // 3. VISTA: DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var result = await _viajeRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Viaje);
        }

        [Authorize(Roles = "AdminTransporte,Administrador,Conductor")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _viajeRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            await CargarListasDesplegables();
            return View(result.Data as Viaje);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador,Conductor")]
        public async Task<IActionResult> Edit(Viaje modelo)
        {
            var originalResult = await _viajeRepository.GetByIdAsync(modelo.Id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var viajeOriginal = originalResult.Data as Viaje;

            viajeOriginal.Estado = modelo.Estado;
            if (modelo.Estado == EstadoViaje.EnCurso && viajeOriginal.HorarioSalidaReal == null)
            {
                viajeOriginal.HorarioSalidaReal = DateTime.Now;
            }
            if (modelo.Estado == EstadoViaje.Completado)
            {
                viajeOriginal.HorarioLlegadaReal = DateTime.Now;
            }

            var result = await _viajeRepository.UpdateEntityAsync(viajeOriginal);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            await CargarListasDesplegables();
            return View(modelo);
        }

        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _viajeRepository.GetByIdAsync(id);
            if (!result.Success || result.Data == null) return NotFound();

            return View(result.Data as Viaje);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminTransporte,Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originalResult = await _viajeRepository.GetByIdAsync(id);
            if (!originalResult.Success || originalResult.Data == null) return NotFound();

            var result = await _viajeRepository.DeleteEntityAsync(originalResult.Data as Viaje);

            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(originalResult.Data as Viaje);
        }

        // --- MÉTODO PARA LLENAR LOS 4 DROPDOWNLISTS ---
        private async Task CargarListasDesplegables()
        {
            var rutas = (await _rutaRepo.GetAllAsync()).Data as IEnumerable<Ruta> ?? new List<Ruta>();
            var buses = (await _autobusRepo.GetAllAsync()).Data as IEnumerable<Autobus> ?? new List<Autobus>();
            var usuarios = (await _usuarioRepo.GetAllAsync()).Data as IEnumerable<Usuario> ?? new List<Usuario>();
            var horarios = (await _horarioRepo.GetAllAsync()).Data as IEnumerable<Horario> ?? new List<Horario>();

            ViewBag.Rutas = new SelectList(rutas, "Id", "NombreRuta");
            ViewBag.Autobuses = new SelectList(buses.Where(b => b.EstadoOperativo == EstadoAutobus.Activo), "Id", "Placa");
            ViewBag.Conductores = new SelectList(usuarios.Where(u => u.Rol == RolUsuario.Conductor), "Id", "NombreCompleto");
            ViewBag.Horarios = new SelectList(horarios, "Id", "HoraSalida");
        }
    }
}