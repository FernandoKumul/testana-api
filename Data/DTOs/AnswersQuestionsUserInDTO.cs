namespace testana_api.Data.DTOs
{
    public class AnswersQuestionsUserInDTO
    {
        public int QuestionId { get; set; }

        public int UserAnswerId { get; set; }

        public string Text { get; set; } = null!;
    }
}
