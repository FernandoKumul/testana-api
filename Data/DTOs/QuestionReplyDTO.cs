using testana_api.Data.Models;

namespace testana_api.Data.DTOs
{
    public class QuestionReplyDTO
    {
        public int Id { get; set; }

        public int? TestId { get; set; }

        public int QuestionTypeId { get; set; }

        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        public int Order { get; set; }

        public bool? CaseSensitivity { get; set; }

        public int Points { get; set; }

        public int? Duration { get; set; }

        public virtual ICollection<QuestionAnswerWithoutCorrectDTO> Answers { get; set; } = new List<QuestionAnswerWithoutCorrectDTO>();
    }
}
