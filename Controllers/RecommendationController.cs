using Microsoft.AspNetCore.Authorization;
using ApplicationCore.DTOs.Recommendation;
using ApplicationCore.DTOs.RecommendationToUpdate;
using Microsoft.AspNetCore.Mvc;
using testana_api.Data.Models;
using testana_api.Services;
using testana_api.Utilities;

namespace testana_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationService _service;

        public RecommendationController(RecommendationService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetById(id);
                if (result == null)
                {
                    return NotFound(new Response<string>(false, $"No se encontró la recomendación con el ID: {id}"));
                }
                return Ok(new Response<Recommendation>(true, "Recomendación obtenida exitosamente", result));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecommendationDto recommendation)
        {
            try
            {
                var result = await _service.Create(recommendation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RecommendationToUpdateDto recommendation)
        {
            if (id != recommendation.Id)
            {
                return BadRequest(new { message = $"El ID: {id} de la URL no coincide con el ID: {recommendation.Id} del cuerpo de la solicitud." });
            }
            try
            {
                var result = await _service.Update(id, recommendation);
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