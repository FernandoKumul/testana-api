using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;
using ApplicationCore.DTOs.Collaborator;
using Microsoft.AspNetCore.Authorization;

namespace testana_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CollaboratorController : ControllerBase
    {
        private readonly CollaboratorService _service;

        public CollaboratorController(CollaboratorService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CollaboratorDto collaborator)
        {
            try
            {
                var result = await _service.Create(collaborator);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CollaboratorDto collaborator)
        {
            if (id != collaborator.Id)
            {
                return BadRequest(new { message = $"El ID: {id} de la URL no coincide con el ID: {collaborator.Id} del cuerpo de la solicitud." });
            }
            try
            {
                var result = await _service.Update(collaborator);
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