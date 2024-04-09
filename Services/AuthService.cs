using ApplicationCore.DTOs.Login;
using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;
using testana_api.Utilities;

namespace testana_api.Services
{
    public class AuthService
    {
        private readonly AppDBContext _context;
        public AuthService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<User?> Login(LoginDto login)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == login.Email && u.Password == UserService.EncryptString(login.Password));
                return user;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al iniciar sesión: {e.Message}", e.InnerException);
            }
        }
        public async Task<Response<User>> Register(UserDto user)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (userExists)
                {
                    return new Response<User>(false, "El correo ya está registrado en la base de datos.");
                }
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = UserService.EncryptString(user.Password),
                    Avatar = ""
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return new Response<User>(true, "Usuario registrado exitosamente", newUser);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al registrar usuario: {e.Message}", e.InnerException);
            }
        }
    }
}