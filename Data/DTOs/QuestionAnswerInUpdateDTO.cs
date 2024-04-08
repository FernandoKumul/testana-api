using System.ComponentModel.DataAnnotations;

namespace testana_api.Data.DTOs
{
    public class QuestionAnswerInUpdateDTO
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        [Required(ErrorMessage = "El texto de la pregunta es obligatoria")]
        public string Text { get; set; } = null!;

        public bool Correct { get; set; }
    }
}
