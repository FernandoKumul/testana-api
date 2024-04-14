using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class AnswerQuestionUserService
    {
        private readonly AppDBContext _context;
        public AnswerQuestionUserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<AnswersQuestionsUser> Create(AnswersQuestionsUserInDTO answer)
        {
            try
            {
                var AnswerFind = await _context.AnswersQuestionsUsers
                    .Where(a => a.QuestionId == answer.QuestionId && a.UserAnswerId == answer.UserAnswerId)
                    .FirstOrDefaultAsync();

                if (AnswerFind != null)
                {
                    throw new Exception("Solo puedes responder una vez cada pregunta");
                }

                var newAnswer = new AnswersQuestionsUser
                {
                    QuestionId = answer.QuestionId,
                    Text = answer.Text,
                    UserAnswerId = answer.UserAnswerId
                };

                await _context.AnswersQuestionsUsers.AddAsync(newAnswer);
                await _context.SaveChangesAsync();
                return newAnswer;
            } catch (Exception)
            {
                throw;
            }

        }
    }
}
