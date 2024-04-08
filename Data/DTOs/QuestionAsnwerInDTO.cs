using System.ComponentModel.DataAnnotations;

namespace testana_api.Data.DTOs
{
    public class QuestionAsnwerInDTO
    {
        [Required(ErrorMessage = "El texto de la pregunta es obligatoria")]
        public string Text { get; set; } = null!;

        public bool Correct { get; set; }
    }
}
