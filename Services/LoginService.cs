using ApplicationCore.DTOs.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;
using testana_api.Utilities;

namespace testana_api.Services
{
    public class LoginService
    {
        private readonly AppDBContext _context;
        public LoginService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Response<User>> Login(LoginDto login)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
                if (user == null)
                {
                    return new Response<User>(false, "Correo o contraseña incorrectos", null);
                }
                return new Response<User>(true, "Inicio de sesión exitoso", user);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al iniciar sesión: {e.Message}");
            }
        }
    }
}