using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SGA_ITLA.WebMVC.Services;
using System.IdentityModel.Tokens.Jwt;

namespace SGA_ITLA.WebMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApi;

        // inyeccion del servicio HTTP en lugar del SgaContext
        public AuthController(IAuthApiService authApi)
        {
            _authApi = authApi;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Manda las credenciales a la API
            var loginResult = await _authApi.LoginAsync(email, password);

            if (loginResult == null || !loginResult.Success)
            {
                ModelState.AddModelError("", "Credenciales incorrectas o cuenta suspendida.");
                return View();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResult.Token);

            var claims = new List<Claim>();
            claims.AddRange(jwtToken.Claims);

            claims.Add(new Claim("jwt_token", loginResult.Token));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}