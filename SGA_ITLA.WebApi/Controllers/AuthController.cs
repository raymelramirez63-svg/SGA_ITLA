using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SGA_ITLA.Infraestructure.Context;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;

namespace SGA_ITLA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SgaContext _context;

        public AuthController(IConfiguration config, SgaContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
          
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email &&
                                          u.PasswordHash == login.Password &&
                                          u.IsActive == true);

            if (usuario != null)
            {
                
                var token = GenerarToken(usuario.Email, usuario.Rol.ToString());
                return Ok(new { success = true, token = token, message = "Autenticación exitosa." });
            }

            return Unauthorized(new { success = false, message = "Credenciales incorrectas o usuario inactivo." });
        }

        private string GenerarToken(string email, string rol)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rol)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}