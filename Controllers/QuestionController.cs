using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using testana_api.Data.DTOs;
using testana_api.Data.Models;
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
            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                int? userId = null;
                if (payloadId != null)
                {
                    userId = int.Parse(payloadId);
                }

                if (answer.QuestionId == 0)
                {
                    return BadRequest(new Response<string>(false, "La Id de la pregunta es obligatoria"));
                }

                var resultCheck = await _service.CheckQuestion(answer, userId);

                return Ok(new Response<CheckOutDTO>(true, "Pregunta evaluado exitosamente", resultCheck));
            } catch (Exception ex)
            {
                if(ex.Message == "Pregunta no encontrado" || ex.Message == "Respuesta no encontrada" || ex.Message == "Respuesta de usuario no encontrada")
                {
                    return NotFound(new Response<string>(false, ex.Message));
                }

                if (ex.Message == "Respuesta obligatoria para una pregunta abierta")
                {
                    return BadRequest(new Response<string>(false, ex.Message));
                }

                if (ex.Message == "Solo puedes responder una vez cada pregunta" || ex.Message == "Ya has completado este test por completo")
                {
                    return Conflict(new Response<string>(false, ex.Message));
                }

                if (ex.Message == "No tienes acceso a la respuesta de usuario que estás proporcionando")
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new Response<string>(false, ex.Message));
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}", ex.InnerException?.Message ?? ""));
            }
        }
    }
}
