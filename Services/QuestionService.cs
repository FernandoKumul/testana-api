using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class QuestionService
    {
        private readonly AppDBContext _context;

        public QuestionService(AppDBContext context)
        {
            _context = context;
        }

        //Mejorar en un futuro :D
        public async Task CreateWithAnswers (List<QuestionInDTO> questions, int? testId)
        {
            try
            {
                foreach (var question in questions)
                {
                    var newQuestion = new Question
                    {
                        TestId = testId,
                        QuestionTypeId = question.QuestionTypeId,
                        Description = question.Description,
                        Image = question.Image,
                        Order = question.Order,
                        CaseSensitivity = question.CaseSensitivity,
                        Points = question.Points,
                        Duration = question.Duration
                    };
                    await _context.Questions.AddAsync(newQuestion);
                    await _context.SaveChangesAsync();

                    foreach (var answer in question.Answers)
                    {
                        var newAsnwer = new QuestionAnswer
                        {
                            QuestionId = newQuestion.Id,
                            Text = answer.Text,
                            Correct = answer.Correct,
                        };
                        await _context.QuestionAnswers.AddAsync(newAsnwer);
                    }

                }
                await _context.SaveChangesAsync();
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
