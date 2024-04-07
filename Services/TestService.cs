using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class TestService
    {
        private readonly AppDBContext _context;
        public TestService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Test>> GetAll()
        {
            return await _context.Tests.ToListAsync();
        }

        public async Task<Test?> GetByIdWithQuestionsAndAnswers(int testId)
        {
            return await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .Where(t => t.Id == testId)
                .FirstOrDefaultAsync();
        }

        public async Task<Test> Create (TestInDTO test)
        {
            var newTest = new Test{
                UserId = test.UserId,
                Title = test.Title,
                Description = test.Description,
                Color = test.Color,
                Visibility = test.Visibility,
                Image = test.Image,
                Status = test.Status,
                Random = test.Random,
                Duration = test.Duration,
                EvaluateByQuestion = test.EvaluateByQuestion,
                Tags = test.Tags,
                CreatedDate = DateTime.Now,
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Tests.AddAsync(newTest);
                await _context.SaveChangesAsync();

                foreach (var question in test.Questions)
                {
                    var newQuestion = new Question
                    {
                        TestId = newTest.Id,
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
                await transaction.CommitAsync();
                return newTest;
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear el test, preguntas y respuestas: ${ex.Message}.", ex.InnerException);
            }
        }

        public async Task<bool> DeleteById (int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var answersToDelete = await _context.QuestionAnswers
                    .Where(a => a.Question.TestId == id)
                    .ToListAsync();
                _context.QuestionAnswers.RemoveRange(answersToDelete);

                var questionsToDelete = await _context.Questions
                    .Where(q => q.TestId == id)
                    .ToListAsync();
                _context.Questions.RemoveRange(questionsToDelete);

                var testToDelete = await _context.Tests.FindAsync(id);
                if (testToDelete != null)
                {
                    _context.Tests.Remove(testToDelete);
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
                throw new Exception($"Error al borrar el test: {ex.Message}", ex.InnerException);
            }

        }
    }
}
