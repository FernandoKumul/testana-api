using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;
using ApplicationCore.DTOs.Login;
using System.IdentityModel.Tokens.Jwt;
using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authorization;

namespace testana_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly UserService _userService;
        private IConfiguration config;

        public AuthController(AuthService service, UserService userService, IConfiguration configuration)
        {
            _service = service;
            _userService = userService;
            config = configuration;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                var user = await _service.Login(login);

                if (user == null)
                {
                    return BadRequest(new Response<User>(false, "Correo o contraseña incorrectos"));
                }

                string token = GenerateToken(user);
                return Ok(new Response<object>(true, "Inicio de sesión exitoso", new { token }));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            try
            {
                var result = await _service.Register(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var SecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return token;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> Get()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null)
            {
                return BadRequest(new Response<string>(false, "Usuario no autenticado"));
            }
            try
            {
                var userId = int.Parse(id);
                var user = await _userService.GetbyId(userId);
                if (user == null)
                {
                    return NotFound(new { message = $"No se encontró el registro con el ID: {userId}" });
                }
                return Ok(new Response<User>(true, "Usuario autenticado", user));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}