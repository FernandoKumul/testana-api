using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;
using testana_api.Utilities;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingEmail != null)
            {
                return new Response<User>(false, "El correo ya está registrado", null);
            }

            try
            {
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = EncryptString(user.Password),
                    Avatar = ""
                };
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
                existingUser.Password = EncryptString(user.Password);
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
            if (user is null)
            {
                return new Response<User>(false, "Registro no encontrado", null);
            }
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return new Response<User>(true, "Registro Eliminado", user);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al eliminar el registro: {e.Message}");
            }
        }

        public static string EncryptString(string str)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
