using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"Error al obtener los registros: {e.Message}");
            }
        }
        public async Task<User?> GetbyId(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al obtener el registro: {e.Message}");
            }
        }
        public async Task<Response<User>> Create(UserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Avatar = ""
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
        public async Task<Response<User>> Update(UserDto user)
        {
            var existingUser = await GetbyId(user.Id);
            if (existingUser == null)
            {
                return new Response<User>(false, "Registro no encontrado", null);
            }
            try
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Avatar = user.Avatar;

                await _context.SaveChangesAsync();
                return new Response<User>(true, "Registro Actualizado", existingUser);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al actualizar el registro: {e.Message}");
            }
        }
        public async Task<Response<User>> Delete(int id)
        {
            var user = await GetbyId(id);
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new Response<User>(true, "Registro Eliminado", user);
            }
            catch (Exception e)
            {
                if (user is null)
                {
                    return new Response<User>(false, "Registro no encontrado", null);
                }
                throw new Exception($"Error al eliminar el registro: {e.Message}");
            }
        }
    }
}
