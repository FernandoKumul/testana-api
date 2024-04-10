using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using testana_api.Data.DTOs;
using testana_api.Services;
using testana_api.Utilities;

namespace testana_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService _service;

        public QuestionController(QuestionService service)
        {
            _service = service;
        }

        [HttpPost("check")]
        public async Task<IActionResult> Check(QuestionCheckDTO answer)
        {
            try
            {
                if (answer.QuestionId == 0) return BadRequest(new Response<string>(false, "La Id de la pregunta es obligatoria"));

                var correct = await _service.CheckQuestion(answer.QuestionId, answer.QuestionAnswerId, answer.OtherAnswer);

                return Ok(new Response<object>(true, "Pregunta evaluado exitosamente", new { correct }));
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}", ex.InnerException?.Message ?? ""));
            }
        }
    }
}
