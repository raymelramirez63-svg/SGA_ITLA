using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.Domain.Entities.Auditoria;
using SGA_ITLA.WebMVC.Services; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGA_ITLA.WebMVC.Controllers
{
    [Authorize(Roles = "Auditor,Administrador,AdminTransporte")]
    public class AuditoriaController : Controller
    {
        private readonly IAuditoriaApiService _auditoriaApi;

        public AuditoriaController(IAuditoriaApiService auditoriaApi)
        {
            _auditoriaApi = auditoriaApi;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _auditoriaApi.ObtenerHistorialAsync();

            var historialOrdenado = lista.OrderByDescending(x => x.CreationDate).ToList();

            return View(historialOrdenado);
        }
    }
}