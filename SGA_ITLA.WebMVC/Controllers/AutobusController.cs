using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.WebMVC.Models;
using System.Text.Json;
using System.Text;

namespace SGA_ITLA.WebMVC.Controllers
{
    public class AutobusController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AutobusController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("SgaApi");
            var response = await client.GetAsync("api/Catalogo/autobus");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var autobuses = JsonSerializer.Deserialize<List<AutobusViewModel>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Ruta absoluta aplicada
                return View("~/Views/Autobus/Index.cshtml", autobuses);
            }

            // Ruta absoluta aplicada en caso de fallo
            return View("~/Views/Autobus/Index.cshtml", new List<AutobusViewModel>());
        }

        // GET: Muestra el formulario
        public IActionResult Create()
        {
            return View("~/Views/Autobus/Create.cshtml");
        }

        // POST: Envía los datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AutobusViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("SgaApi");
                var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/Catalogo/autobus", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Error al guardar en la API. Revisa que el token no sea necesario aún o que los datos estén correctos.");
            }

            return View("~/Views/Autobus/Create.cshtml", model);
        }
    }
}