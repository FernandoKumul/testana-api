using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<ActionResult<Response<Test>>> Create([FromBody] TestInDTO test)
        {
            try
            {
                //Validar si el id del usuario existe -> Luego

                for (int i = 0; i < test.Questions.Count; i++)
                {
                    int nCorrect = 0;
                    foreach(var answer in test.Questions[i].Answers)
                    {
                        if (answer.Correct) nCorrect++;
                    }
                    
                    if(nCorrect != 1) {
                        return BadRequest(new Response<string>(false, $"La pregunta[{i}] no tiene ninguna respuesta correcta"));
                    }
                }

                string[] visibilityTypes = { "unlisted", "private", "public" };
                if (!Array.Exists(visibilityTypes, color => color == test.Visibility))
                {
                    BadRequest(new Response<int>(false, "No está ingresando algún tipo de visibilidad valido"));
                }

                string[] colors = { "green", "blue", "purple", "orange", "yellow", "red" };    
                
                if (!Array.Exists(colors, color => color == test.Color))
                {
                    BadRequest(new Response<int>(false, "No está ingresando algún color valido"));
                }
                
                //Agregar en la función
                var newTest = await _service.Create(test);
                return Created("created test", new Response<Test>(true, "Test creado con exito", newTest));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }
    }
}
