using System.ComponentModel.DataAnnotations;
using testana_api.Data.Models;

namespace testana_api.Data.DTOs
{
    public class QuestionInDTO
    {
        [Range(1, 2, ErrorMessage = "Tipo de pregrunta no valida")]
        public int QuestionTypeId { get; set; }

        [Required(ErrorMessage = "La descripción de la pregunta es obligatoria")]
        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El orden de la pregunta es obligatoria.")]
        public int Order { get; set; }

        public bool? CaseSensitivity { get; set; }

        public int Points { get; set; }

        public int? Duration { get; set; }

        public List<QuestionAsnwerInDTO> Answers { get; set; } = new List<QuestionAsnwerInDTO>();

    }
}
