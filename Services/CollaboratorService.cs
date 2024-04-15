using ApplicationCore.DTOs.Collaborator;
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
        public async Task<Collaborator?> GetById(int id)
        {
            try
            {
                return await _context.Collaborators.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener colaborador: {ex.Message}", ex.InnerException);
            }
        }
        public async Task<Response<Collaborator>> Create(CollaboratorDto collaborator)
        {
            var repetedCollaborator = await _context.Collaborators.FirstOrDefaultAsync(c => c.TestId == collaborator.TestId && c.UserId == collaborator.UserId);
            if (repetedCollaborator != null)
            {
                return new Response<Collaborator>(false, "El colaborador ya está registrado");
            }
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
        public async Task<Response<Collaborator>> Update(CollaboratorDto collaborator)
        {
            var existingCollaborator = await GetById(collaborator.Id);
            if (existingCollaborator == null)
            {
                return new Response<Collaborator>(false, "Colaborador no encontrado");
            }
            try
            {
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
        public async Task<bool> Delete(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var recommendationsToDelete = await _context.Recommendations.Where(r => r.CollaboratorId == id).ToListAsync();
                _context.Recommendations.RemoveRange(recommendationsToDelete);
                var collaborator = await GetById(id);
                if (collaborator != null)
                {
                    _context.Collaborators.Remove(collaborator);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al eliminar colaborador: {ex.Message}", ex.InnerException);
            }
        }
    }
}