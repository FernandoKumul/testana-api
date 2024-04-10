using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;


namespace testana_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _service.GetAll();
                return Ok(new Response<IEnumerable<User>>(true, "Datos obtenidos exitosamente", users));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetbyId(int id)
        {
            try
            {
                var user = await _service.GetbyId(id);
                if (user == null)
                {
                    return NotFound(new { message = $"No se encontró el registro con el ID: {id}" });
                }
                return Ok(new Response<User>(true, "Datos obtenidos exitosamente", user));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto user)
        {
            try
            {
                var result = await _service.Create(user);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto user)
        {
            try
            {
                if (id != user.Id)
                {
                    return BadRequest(new { message = $"El ID: {id} de la URL no coincide con el ID: {user.Id} del cuerpo de la solicitud." });
                }
                var result = await _service.Update(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

    }
}