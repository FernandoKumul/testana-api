using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using testana_api.Utilities;

namespace testana_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        public UploadController(IConfiguration configuration) 
        {
            var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        [HttpPost("image")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new Response<string>(false, "Ningún archivo subido"));
            }

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return BadRequest(new Response<string>(false, $"Error al subir la imagen: {uploadResult.Error.Message}"));
                }

                return Ok(new Response<object>(true, "Imagen subida exitosamente", new { url = uploadResult.Url }));
            }
        }
    }
}
