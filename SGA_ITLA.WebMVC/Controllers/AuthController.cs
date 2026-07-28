using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SGA_ITLA.WebMVC.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace SGA_ITLA.WebMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Muestra la pantalla de Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Envía las credenciales a la API
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("SgaApi");

            var authData = new { email = model.Correo, password = model.Clave };
            var jsonContent = new StringContent(JsonSerializer.Serialize(authData), Encoding.UTF8, "application/json");

            // Llamada al endpoint de la API
            var response = await client.PostAsync("api/Auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Extraemos el token del JSON que devuelve la API
                using var jsonDoc = JsonDocument.Parse(content);

                var token = jsonDoc.RootElement.GetProperty("token").GetString()!;

                // Guardamos el token en las Cookies del navegador
                await IniciarSesionEnMVC(token);

                // Redirigir al inicio o al catálogo
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Credenciales incorrectas o error de conexión con la API.");
            return View(model);
        }

        // GET: Cerrar sesión
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        // Método privado que desencripta el JWT y crea la sesión en MVC
        private async Task IniciarSesionEnMVC(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var claims = new List<Claim>();
            claims.AddRange(jwt.Claims);

            // Guardamos el token original en un Claim para usarlo luego en otras peticiones
            claims.Add(new Claim("jwt_token", token));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = jwt.ValidTo // La sesión web expira exactamente cuando expira el token de la API
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}