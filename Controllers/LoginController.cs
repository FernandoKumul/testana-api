using Microsoft.AspNetCore.Mvc;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;
using ApplicationCore.DTOs.Login;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace testana_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _service;
        private IConfiguration config;

        public LoginController(LoginService service, IConfiguration configuration)
        {
            _service = service;
            config = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            try
            {
                var user = await _service.Login(login);

                if(user == null)
                {
                    return BadRequest(new Response<User>(false, "Correo o contraseña incorrectos"));
                }

                string token = GenerateToken(user);
                return Ok(new Response<object>(true, "Inicio de sesión exitoso", new { token }));
            } catch (Exception ex)
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
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var SecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return token;
        }
    }
}