using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class TestService
    {
        private readonly AppDBContext _context;
        private readonly QuestionService _questionService;

        public TestService(AppDBContext context, QuestionService questionService)
        {
            _context = context;
            _questionService = questionService;
        }

        public async Task<IEnumerable<Test>> GetAll()
        {
            return await _context.Tests.ToListAsync();
        }

        public async Task<Test?> GetByIdQuestionsAnswers(int testId)
        {
            return await _context.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .Where(t => t.Id == testId)
                .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Test>, int)> Search(int pageNumber, int pageSize, string textSearch)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;

                var pagedData = await _context.Tests
                    .Where(t => EF.Functions.Like(t.Title, $"%{textSearch}%") ||
                        EF.Functions.Like(t.Tags, $"%{textSearch}%"))
                    .OrderBy(x => x.Likes)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();
                var count = _context.Tests.
                    Count(t => EF.Functions.Like(t.Title, $"%{textSearch}%") || 
                        EF.Functions.Like(t.Tags, $"%{textSearch}%"));

                return (pagedData, count);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al borrar el test: {ex.Message}", ex.InnerException);
            }
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

                await _questionService.CreateWithAnswers(test.Questions, newTest.Id);

                await transaction.CommitAsync();
                return newTest;
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al crear el test, preguntas y respuestas: ${ex.Message}.", ex.InnerException);
            }
        }

        public async Task<bool> UpdateQuestionsAnswers(TestInUpdateDTO updatedTest, int idTest)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var idQuestionAdds = new List<int>();
                var existingTest = await GetByIdQuestionsAnswers(idTest);

                if (existingTest is null)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                existingTest.Title = updatedTest.Title;
                existingTest.Description = updatedTest.Description;
                existingTest.Color = updatedTest.Color;
                existingTest.Visibility = updatedTest.Visibility;
                existingTest.Image = updatedTest.Image;
                existingTest.Status = updatedTest.Status;
                existingTest.Random = updatedTest.Random;
                existingTest.Duration = updatedTest.Duration;
                existingTest.EvaluateByQuestion = updatedTest.EvaluateByQuestion;
                existingTest.Tags = updatedTest.Tags;

                foreach (var updatedQuestion in updatedTest.Questions)
                {
                    var existingQuestion = existingTest.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);

                    if (existingQuestion != null)
                    {
                        //Actualizo pregunta
                        idQuestionAdds.Add(existingQuestion.Id);
                        existingQuestion.QuestionTypeId = updatedQuestion.QuestionTypeId;
                        existingQuestion.Description = updatedQuestion.Description;
                        existingQuestion.Image = updatedQuestion.Image;
                        existingQuestion.Order = updatedQuestion.Order;
                        existingQuestion.CaseSensitivity = updatedQuestion.CaseSensitivity;
                        existingQuestion.Points = updatedQuestion.Points;
                        existingQuestion.Duration = updatedQuestion.Duration;

                        foreach (var updatedAnswer in updatedQuestion.Answers)
                        {
                            var existingAnswer = existingQuestion.Answers.FirstOrDefault(a => a.Id == updatedAnswer.Id);
                            if (existingAnswer != null)
                            {
                                //Actualizar respuesta
                                existingAnswer.Text = updatedAnswer.Text;
                                existingAnswer.Correct = updatedAnswer.Correct;
                            }
                            else
                            {
                                // Insertar nueva respuesta
                                var newAnswer = new QuestionAnswer
                                {
                                    Correct = updatedAnswer.Correct,
                                    Text = updatedAnswer.Text,
                                    QuestionId = existingQuestion.Id
                                };
                                existingQuestion.Answers.Add(newAnswer);
                            }
                        }

                        List<QuestionAnswer> answerList = existingQuestion.Answers.ToList();
                        answerList.RemoveAll(q => updatedQuestion.Answers.Exists(uq => uq.Id == q.Id));
                        _context.QuestionAnswers.RemoveRange(answerList);
                    }
                    else
                    {
                        //Agrega nueva pregunta con respuestas
                        var newQuestion = new Question
                        {
                            TestId = idTest,
                            QuestionTypeId = updatedQuestion.QuestionTypeId,
                            Description = updatedQuestion.Description,
                            Image = updatedQuestion.Image,
                            Order = updatedQuestion.Order,
                            CaseSensitivity = updatedQuestion.CaseSensitivity,
                            Points = updatedQuestion.Points,
                            Duration = updatedQuestion.Duration
                        };
                        await _context.Questions.AddAsync(newQuestion);
                        await _context.SaveChangesAsync();
                        idQuestionAdds.Add(newQuestion.Id);

                        foreach (var answer in updatedQuestion.Answers)
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
                }
                await _context.SaveChangesAsync();

                //Borra las preguntas y respuestas que ya no incluya
                var idQuestionDelete = new List<int>();
                foreach(var question in existingTest.Questions)
                {
                    if (!idQuestionAdds.Contains(question.Id)) idQuestionDelete.Add(question.Id); 
                }

                var answersToDelete = await _context.QuestionAnswers
                    .Where(a => idQuestionDelete.Contains(a.QuestionId))
                    .ToListAsync();
                _context.QuestionAnswers.RemoveRange(answersToDelete);

                var questionsToDelete = await _context.Questions
                    .Where(q => idQuestionDelete.Contains(q.Id))
                    .ToListAsync();
                _context.Questions.RemoveRange(questionsToDelete);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
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
