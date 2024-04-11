﻿using Microsoft.AspNetCore.Mvc;
using testana_api.Data.DTOs;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;

namespace testana_api.Controllers
{
    [ApiController]
    [Route("api/user-answer")]
    public class UserAnswerController : ControllerBase
    {
        private readonly UsersAnswerService _service;
        public UserAnswerController(UsersAnswerService usersAnswerService)
        {
            _service = usersAnswerService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<UsersAnswer>>> Create([FromBody] UserAnswerInDTO userAnswer)
        {
            try
            {
                var newUserAnswer = await _service.Create(userAnswer);
                return CreatedAtAction(null, 
                    new Response<UsersAnswer>(true, "Respuesta de Usuario creado de manera exitosa", newUserAnswer));
            } catch (Exception ex)
            {
                if (ex.Message == "Test no encontrado")
                {
                    return NotFound(new Response<string>(false, $"{ex.Message} con el id {userAnswer.TestId}"));
                }

                if (ex.Message == "Usuario no encontrado")
                {
                    return NotFound(new Response<string>(false, $"{ex.Message} con el id {userAnswer.UserId}"));
                }

                if (ex.Message == "Necesitas ingresar un nombre para realizar el test")
                {
                    return BadRequest(new Response<string>(false, ex.Message));
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<string>(false, $"Error al obtener los datos: {ex.Message}", ex.InnerException?.Message ?? ""));
            }
        }
    }
}
