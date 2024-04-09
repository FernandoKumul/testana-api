using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;


namespace testana_api.Controllers{
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
            var users = await _service.GetAll();
            return Ok(new Response<IEnumerable<User>>(true, "Datos obtenidos exitosamente", users));
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetbyId(int id)
        {
            var user = await _service.GetbyId(id);
            if (user == null)
            {
                return NotFound(new { message = $"El usuario con ID: {id} no existe en la base de datos." });
            }
            return Ok(new Response<User>(true, "Usuario encontrado en la base de datos", user));
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto user)
        {
            var result = await _service.Create(user);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest(new { message = $"El ID: {id} de la URL no coincide con el ID: {user.Id} del cuerpo de la solicitud." });
            }
            var result = await _service.Update(user);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

    }
}