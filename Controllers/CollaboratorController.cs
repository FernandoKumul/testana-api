using Microsoft.AspNetCore.Mvc;
using testana_api.Services;
using testana_api.Utilities;
using ApplicationCore.DTOs.Collaborator;
using Microsoft.AspNetCore.Authorization;
using testana_api.Data.Models;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<Collaborator>> GetbyId(int id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null)
                {
                    return NotFound(new Response<string>(false, $"No se encontró el colaborador con el ID: {id}"));
                }
                return Ok(new Response<Collaborator>(true, "Colaborador obtenido exitosamente", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
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
        public async Task<ActionResult<Response<string>>> Delete (int id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (!result)
                {
                    return NotFound(new Response<string>(false, $"El Colaborador con el id {id} no existe"));
                }

                return Ok(new Response<string>(true, "Colaborador borrado de manera exitosa"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}