using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;
using testana_api.Utilities;

namespace testana_api.Services
{
    public class UserService
    {
        private readonly AppDBContext _context;
        public UserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetbyId(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<Response<User>> Create(UserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Avatar = user.Avatar
            };
            try
            {
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return new Response<User>(true, "Registro Creado", newUser);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al crear el registro: {e.Message}");
            }
        }
    }
}
