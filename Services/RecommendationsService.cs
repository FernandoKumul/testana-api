using testana_api.Data;
using ApplicationCore.DTOs.Recommendation;
using ApplicationCore.DTOs.RecommendationToUpdate;
using testana_api.Data.Models;
using testana_api.Utilities;

namespace testana_api.Services
{
    public class RecommendationService
    {
        private readonly AppDBContext _context;
        public RecommendationService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Recommendation?> GetById(int id)
        {
            try
            {

                return await _context.Recommendations.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la recomendación", ex);
            }
        }

        public async Task<Response<Recommendation>> Create(RecommendationDto recommendation)
        {
            try
            {
                var newRecommendation = new Recommendation
                {
                    CollaboratorId = recommendation.CollaboratorId,
                    QuestionId = recommendation.QuestionId,
                    Note = recommendation.Note,
                    CreatedDate = DateTime.Now
                };
                await _context.Recommendations.AddAsync(newRecommendation);
                await _context.SaveChangesAsync();
                return new Response<Recommendation>(true, "Recomendación creada exitosamente", newRecommendation);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la recomendación", ex);
            }
        }

        public async Task<Response<Recommendation>> Update(int id, RecommendationToUpdateDto recommendation)
        {
            var recommendationToUpdate = await GetById(id);
            if (recommendationToUpdate == null)
            {
                return new Response<Recommendation>(false, "Recomendación no encontrada");
            }
            try
            {
                recommendationToUpdate.Note = recommendation.Note;
                recommendationToUpdate.CreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return new Response<Recommendation>(true, "Recomendación actualizada exitosamente", recommendationToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la recomendación", ex);
            }
        }

        public async Task<Response<Recommendation>> Delete(int id)
        {
            var recommendationToDelete = await GetById(id);
            if (recommendationToDelete == null)
            {
                return new Response<Recommendation>(false, "Recomendación no encontrada");
            }
            try
            {
                _context.Recommendations.Remove(recommendationToDelete);
                await _context.SaveChangesAsync();
                return new Response<Recommendation>(true, "Recomendación eliminada exitosamente", recommendationToDelete);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la recomendación", ex);
            }
        }
    }
}