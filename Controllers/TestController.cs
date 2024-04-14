using Microsoft.AspNetCore.Authorization;
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
    public class TestController : ControllerBase
    {
        private readonly TestService _service;

        public TestController(TestService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tests = await _service.GetAll();
            return Ok(new Response<IEnumerable<Test>>(true, "Datos obtenidos exitosamente", tests));
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParameters parameters)
        {
            var pageNumber = parameters.PageNumber;
            var pageSize = parameters.PageSize;
            try
            {
                (object result, int count) = await _service
                    .Search(pageNumber, pageSize, parameters.Search);

                return Ok(new Response<object>(true, "Datos obtenidos exitosamente", new {result, count }));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}"));
            }

        }

        [Authorize]
        [HttpGet("questions-answers/{id}")]
        public async Task<IActionResult> GetByIdQuestionsAnswers(int id)
        {
            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (payloadId == null)
                {
                    return BadRequest(new Response<string>(false, "Usuario no autenticado"));
                }

                var userId = int.Parse(payloadId);
                var test = await _service.GetByIdQuestionsAnswers(id, userId);
                if(test is null) 
                {
                    return NotFound(new Response<string>(false, $"Test no encontrado con el id {id}"));
                }

                return Ok(new Response<Test>(true, "Datos obtenidos exitosamente", test));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }       
        [Authorize]
        [HttpGet("done")]
        public async Task<IActionResult> GetDoneByUserId()
        {
            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (payloadId == null)
                {
                    return BadRequest(new Response<string>(false, "Usuario no autenticado"));
                }

                var userId = int.Parse(payloadId);
                var test = await _service.GetDoneByUserId(userId);

                return Ok(new Response<IEnumerable<TestMinOutDTO>>(true, "Datos obtenidos exitosamente", test));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
        
        [Authorize]
        [HttpGet("created")]
        public async Task<IActionResult> GetCreatedByUserId()
        {
            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (payloadId == null)
                {
                    return BadRequest(new Response<string>(false, "Usuario no autenticado"));
                }

                var userId = int.Parse(payloadId);
                var test = await _service.GetCreatedByUserId(userId);

                return Ok(new Response<IEnumerable<TestMinOutDTO>>(true, "Datos obtenidos exitosamente", test));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [HttpGet("preview/{id}")]
        public async Task<IActionResult> GetPreviewById(int id)
        {
            try
            {
                var idPayload = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int? userId = null;

                if(idPayload != null)
                {
                    userId = int.Parse(idPayload);
                }

                var test = await _service.GetPreviewById(id, userId);
                if (test is null)
                {
                    return NotFound(new Response<string>(false, $"Test no encontrado con el id: {id}"));
                }

                return Ok(new Response<TestPreviewOutDTO>(true, "Test obtenido exitosamente", test));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}", ex.InnerException?.Message ?? ""));
            }
        }

        [HttpGet("reply-one/{id}")]
        public async Task<IActionResult> GetReplyOneById(int id)
        {
            try
            {
                var idPayload = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int? userId = null;

                if (idPayload != null)
                {
                    userId = int.Parse(idPayload);
                }

                var test = await _service.GetReplyOneById(id, userId);
                if (test is null)
                {
                    return NotFound(new Response<string>(false, $"Test no encontrado con el id: {id}"));
                }

                return Ok(new Response<TestReplyOutDTO>(true, "Test obtenido exitosamente", test));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}", ex.InnerException?.Message ?? ""));
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Response<Test>>> Create([FromBody] TestInDTO test)
        {
            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (payloadId == null)
                {
                    return BadRequest(new Response<string>(false, "Usuario no autenticado"));
                }

                var userId = int.Parse(payloadId);

                if (userId != test.UserId)
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new Response<string>(false, "El userId no coincide con su sesión"));
                }

                for (int i = 0; i < test.Questions.Count; i++)
                {
                    int nCorrect = 0;
                    foreach (var answer in test.Questions[i].Answers)
                    {
                        if (answer.Correct) nCorrect++;
                    }

                    if (nCorrect != 1) {
                        return BadRequest(new Response<string>(false, $"La pregunta[{i}] no tiene ninguna respuesta correcta"));
                    }
                }

                string[] visibilityTypes = { "unlisted", "private", "public" };
                if (!Array.Exists(visibilityTypes, color => color == test.Visibility))
                {
                    return BadRequest(new Response<int>(false, "No está ingresando algún tipo de visibilidad valido"));
                }

                string[] colors = { "green", "blue", "purple", "orange", "yellow", "red" };

                if (!Array.Exists(colors, color => color == test.Color))
                {
                    return BadRequest(new Response<int>(false, "No está ingresando algún color valido"));
                }

                //Agregar en la función
                var newTest = await _service.Create(test);
                return Created("created test", new Response<Test>(true, "Test creado con exito", newTest));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [Authorize]
        [HttpPut("questions-answers/{id}")]
        public async Task<ActionResult<Response<string>>> Update (int id, [FromBody] TestInUpdateDTO updateTest)
        {

            for (int i = 0; i < updateTest.Questions.Count; i++)
            {
                int nCorrect = 0;
                foreach (var answer in updateTest.Questions[i].Answers)
                {
                    if (answer.Correct) nCorrect++;
                }

                if (nCorrect != 1)
                {
                    return BadRequest(new Response<string>(false, $"La pregunta[{i}] no tiene ninguna respuesta correcta"));
                }
            }

            string[] visibilityTypes = { "unlisted", "private", "public" };
            if (!Array.Exists(visibilityTypes, color => color == updateTest.Visibility))
            {
                return BadRequest(new Response<int>(false, "No está ingresando algún tipo de visibilidad valido"));
            }

            string[] colors = { "green", "blue", "purple", "orange", "yellow", "red" };

            if (!Array.Exists(colors, color => color == updateTest.Color))
            {
                return BadRequest(new Response<int>(false, "No está ingresando algún color valido"));
            }

            var payloadId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                if (payloadId == null)
                {
                    return BadRequest(new Response<string>(false, "Usuario no autenticado"));
                }

                var userId = int.Parse(payloadId);

                var result = await _service.UpdateQuestionsAnswers(updateTest, id, userId);

                if (!result)
                {
                    return NotFound(new Response<string>(false, $"El test con el id {id} no existe"));
                }

                return Ok(new Response<string>(true, "Test Actualizado de manera exitosa"));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> Delete (int id)
        {
            try
            {
                var result = await _service.DeleteById(id);

                if (!result)
                {
                    return NotFound(new Response<string>(false, $"El test con el id {id} no existe"));
                }

                return Ok(new Response<string>(true, "Test borrado de manera exitosa"));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
    }
}
