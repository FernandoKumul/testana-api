using Microsoft.EntityFrameworkCore;
using testana_api.Data;
using testana_api.Data.DTOs;
using testana_api.Data.Models;

namespace testana_api.Services
{
    public class UsersAnswerService
    {
        private readonly AppDBContext _context;
        public UsersAnswerService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<UsersAnswer> Create(UserAnswerInDTO userAnswer)
        {
            try
            {
                var testFind = await _context.Tests.FindAsync(userAnswer.TestId) ??
                    throw new Exception("Test no encontrado");

                if(!testFind.Status)
                {
                    throw new Exception("Test no encontrado");
                }

                //Verificar la visibilidad privada

                //Verificar que el si se puede responser por respuesta o por completado todo 

                var newUserAnswer = new UsersAnswer
                {
                    TestId = userAnswer.TestId,
                };

                if (userAnswer.UserId != null)
                {
                    var userFind = await _context.Users.FindAsync(userAnswer.UserId) ?? 
                        throw new Exception("Usuario no encontrado");

                    newUserAnswer.Name = null;
                    newUserAnswer.UserId = userAnswer.UserId;
                } else
                {
                    if (userAnswer.Name is null) 
                        throw new Exception("Necesitas ingresar un nombre para realizar el test");

                    newUserAnswer.Name = userAnswer.Name;
                    newUserAnswer.UserId = null;
                }


                await _context.UsersAnswers.AddAsync(newUserAnswer);
                await _context.SaveChangesAsync();
                return newUserAnswer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
