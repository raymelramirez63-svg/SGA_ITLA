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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.IdentificacionInstitucional))
                {
                    return BadRequest(new { success = false, message = "La Identificación Institucional es obligatoria." });
                }

                var existeEmail = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
                if (existeEmail)
                {
                    return BadRequest(new { success = false, message = "El correo ya está registrado." });
                }

                var existeIdentificacion = await _context.Usuarios.AnyAsync(u => u.IdentificacionInstitucional == dto.IdentificacionInstitucional);
                if (existeIdentificacion)
                {
                    return BadRequest(new { success = false, message = $"La Identificación Institucional '{dto.IdentificacionInstitucional}' ya se encuentra registrada." });
                }

                var nuevoUsuario = new Usuario
                {
                    IdentificacionInstitucional = dto.IdentificacionInstitucional,
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    Rol = dto.Rol,
                    IsActive = true
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Usuario registrado correctamente cumpliendo RF-USU-01." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error de BD: {ex.InnerException?.Message ?? ex.Message}" });
            }
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

    public class RegisterDto
    {
        public string IdentificacionInstitucional { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
    }
}