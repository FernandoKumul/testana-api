using ApplicationCore.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;

namespace testana_api.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }
        // Get for all users in the table Users in tha database testana
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAll();
            return Ok(new Response<IEnumerable<User>>(true, "Datos obtenidos exitosamente", users));
        }
        // Get by id for a user in the table Users in tha database testana, the id is the primary key in the table, use this for get a specific user
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetbyId(int id)
        {
            var user = await _service.GetbyId(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new Response<User>(true, "Usuario encontrado en la base de datos", user));
        }
        // Post for create a new user in the table Users in tha database testana
        [HttpPost]
        public async Task<IActionResult> Create(UserDto user)
        {
            var result = await _service.Create(user);
            return Ok(result);
        }
        // Put for update a specific user in the table Users in tha database testana
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            var result = await _service.Update(user);
            return Ok(result);
        }
    }
}