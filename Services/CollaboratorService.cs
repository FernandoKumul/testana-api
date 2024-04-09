using ApplicationCore.DTOs.Collaborator;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.Models;
using testana_api.Utilities;

namespace testana_api.Services
{
    public class CollaboratorService
    {
        private readonly AppDBContext _context;
        public CollaboratorService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Response<Collaborator>> Create(CollaboratorDto collaborator){
            try
            {
                var newCollaborator = new Collaborator
                {
                    TestId = collaborator.TestId,
                    UserId = collaborator.UserId
                };
                await _context.Collaborators.AddAsync(newCollaborator);
                await _context.SaveChangesAsync();
                return new Response<Collaborator>(true, "Colaborador creado exitosamente", newCollaborator);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar usuario: {ex.Message}", ex.InnerException);
            }
        }
        public async Task<Response<Collaborator>> Update(CollaboratorDto collaborator){
            try
            {
                var existingCollaborator = await _context.Collaborators.FirstOrDefaultAsync(c => c.TestId == collaborator.TestId && c.UserId == collaborator.UserId);
                if (existingCollaborator == null)
                {
                    throw new Exception("Colaborador no encontrado");
                }
                existingCollaborator.TestId = collaborator.TestId;
                existingCollaborator.UserId = collaborator.UserId;
                _context.Collaborators.Update(existingCollaborator);
                await _context.SaveChangesAsync();
                return new Response<Collaborator>(true, "Colaborador actualizado exitosamente", existingCollaborator);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar colaborador: {ex.Message}", ex.InnerException);
            }
        }
        public async Task<Response<Collaborator>> Delete(CollaboratorDto collaborator){
            try
            {
                var existingCollaborator = await _context.Collaborators.FirstOrDefaultAsync(c => c.TestId == collaborator.TestId && c.UserId == collaborator.UserId);
                if (existingCollaborator == null)
                {
                    throw new Exception("Colaborador no encontrado");
                }
                _context.Collaborators.Remove(existingCollaborator);
                await _context.SaveChangesAsync();
                return new Response<Collaborator>(true, "Colaborador eliminado exitosamente", existingCollaborator);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar colaborador: {ex.Message}", ex.InnerException);
            }
        }
    }
}