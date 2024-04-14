using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class QuestionService
    {
        private readonly AppDBContext _context;
        private readonly AnswerQuestionUserService _answerQuestionUserService;
        private readonly UsersAnswerService _usersAnswerService;

        public QuestionService(AppDBContext context, AnswerQuestionUserService answerQuestionUserService, UsersAnswerService usersAnswerService)
        {
            _context = context;
            _answerQuestionUserService = answerQuestionUserService;
            _usersAnswerService = usersAnswerService;
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

        //Se podría envolver en una transacción
        public async Task<CheckOutDTO> CheckQuestion (QuestionCheckDTO check, int? userId)
        {
            try
            {
                var UserAnswer = await _context.UsersAnswers.FindAsync(check.UserAnswerId) ??
                    throw new Exception("Respuesta de usuario no encontrada");

                if(UserAnswer.CompletionDate != null)
                {
                    throw new Exception("Ya has completado este test por completo");
                }

                if (UserAnswer.UserId != userId)
                {
                    throw new Exception("No tienes acceso a la respuesta de usuario que estás proporcionando");
                }

                var question = await _context.Questions
                    .Include(q => q.Answers)
                    .FirstOrDefaultAsync(q => q.Id == check.QuestionId && UserAnswer.TestId == q.TestId);

                if (question == null)
                {
                    throw new Exception("Pregunta no encontrada");
                }

                //Preguntas opcíón multiple
                if (question.QuestionTypeId == 2)
                {
                    foreach (var answer in question.Answers)
                    {
                        if (answer.Id == check.QuestionAnswerId)
                        {
                            var newAnswer = new AnswersQuestionsUserInDTO
                            {
                                QuestionId = answer.QuestionId,
                                Text = answer.Text,
                                UserAnswerId = check.UserAnswerId
                            };

                            await _answerQuestionUserService.Create(newAnswer);

                            int count = await _context.UsersAnswers
                                .Where(ua => ua.Id == check.UserAnswerId)
                                .SelectMany(ua => ua.AnswersQuestionsUsers)
                                .CountAsync();

                            var completeTest = count == await _context.Questions
                                .Where(q => q.TestId == UserAnswer.TestId).CountAsync();

                            if (completeTest)
                            {
                                await _usersAnswerService.Complete(check.UserAnswerId);
                            }

                            return new CheckOutDTO
                            {
                                Correct = answer.Correct,
                                Complete = completeTest
                            };
                        }
                    }

                    throw new Exception("Respuesta no encontrada");
                }
                
                //Pregunta abierta
                if (question.QuestionTypeId == 1)
                {
                    if (check.OtherAnswer is null) throw new Exception("Respuesta obligatoria para una pregunta abierta");

                    var correct = false;
                    foreach (var answer in question.Answers)
                    {
                        if (question.CaseSensitivity == true)
                        {
                            if (answer.Text == check.OtherAnswer)
                            {
                                correct = true;
                                break;
                            }
                        }
                        else
                        {
                            if (answer.Text.ToLower() == check.OtherAnswer.ToLower())
                            {
                                correct = true;
                                break;
                            }
                        }
                    }

                    var newAnswer = new AnswersQuestionsUserInDTO
                    {
                        QuestionId = question.Id,
                        Text = check.OtherAnswer,
                        UserAnswerId = check.UserAnswerId
                    };

                    await _answerQuestionUserService.Create(newAnswer);

                    int count = await _context.UsersAnswers
                        .Where(ua => ua.Id == check.UserAnswerId)
                        .SelectMany(ua => ua.AnswersQuestionsUsers)
                        .CountAsync();

                    var completeTest = count == await _context.Questions
                        .Where(q => q.TestId == UserAnswer.TestId).CountAsync();

                    if (completeTest)
                    {
                        await _usersAnswerService.Complete(check.UserAnswerId);
                    }

                    return new CheckOutDTO
                    {
                        Correct = correct,
                        Complete = completeTest
                    };
                }

                throw new Exception("Tipo de pregunta no encontrada");

            } catch (Exception)
            {
                throw;
            }
        }
    
    }
}
