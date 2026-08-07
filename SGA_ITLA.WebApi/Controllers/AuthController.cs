using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SGA_ITLA.Application.Interfaces.Usuarios;
using SGA_ITLA.Domain.Entities.Usuarios;
using SGA_ITLA.Domain.Enums;
using SGA_ITLA.Domain.Interfaces;

namespace SGA_ITLA.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthController(IConfiguration config, IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var usuario = await _usuarioService.ValidarCredencialesAsync(login.Email, login.Password);

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

                var usuarioExistente = await _usuarioRepository.GetByEmailAsync(dto.Email);
                if (usuarioExistente != null)
                {
                    return BadRequest(new { success = false, message = "El correo ya está registrado." });
                }

                var existeIdentificacion = await _usuarioRepository.ExisteIdentificacionAsync(dto.IdentificacionInstitucional);
                if (existeIdentificacion)
                {
                    return BadRequest(new { success = false, message = $"La Identificación Institucional '{dto.IdentificacionInstitucional}' ya se encuentra registrada." });
                }

                var nuevoUsuario = new Usuario
                {
                    IdentificacionInstitucional = dto.IdentificacionInstitucional,
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Rol = dto.Rol,
                    IsActive = true
                };

                await _usuarioRepository.SaveEntityAsync(nuevoUsuario);

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