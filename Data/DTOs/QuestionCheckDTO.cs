using System.ComponentModel.DataAnnotations;

namespace testana_api.Data.DTOs
{
    public class QuestionCheckDTO
    {
        public int? QuestionAnswerId { get; set; }

        public int QuestionId { get; set; }

        public string? OtherAnswer { get; set; }

        public int UserAnswerId { get; set; }
    }
}
