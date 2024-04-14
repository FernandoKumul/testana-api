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

        public async Task<bool> CheckQuestion (int questionId , int? answerId, string? otherText)
        {
            try
            {
                var question = await _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Id == questionId);

                if (question == null)
                {
                    throw new Exception("Pregunta no encontrada");
                }

                //Preguntas opcíón multiple
                if (question.QuestionTypeId == 2)
                {
                    foreach (var answer in question.Answers)
                    {
                        if (answer.Id == answerId)
                        {
                            //Registrar en DB
                            return answer.Correct;
                        }
                    }

                    throw new Exception("Respuesta no encontrada");
                }
                
                //Pregunta abierta
                if (question.QuestionTypeId == 1)
                {
                    if (otherText is null) throw new Exception("Respuesta obligatoria para una pregunta abierta");

                    foreach (var answer in question.Answers)
                    {
                        if (question.CaseSensitivity == true)
                        {
                            if (answer.Text == otherText) return true;
                        }
                        else
                        {
                            if (answer.Text.ToLower() == otherText.ToLower()) return true;
                        }
                    }
                    //Registrar en DB
                    return false;
                }

                throw new Exception("Tipo de pregunta no encontrada");

            } catch (Exception)
            {
                throw;
            }
        }
    
    }
}
