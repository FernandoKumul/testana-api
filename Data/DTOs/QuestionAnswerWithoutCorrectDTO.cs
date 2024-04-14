namespace testana_api.Data.DTOs
{
    public class QuestionAnswerWithoutCorrectDTO
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public string Text { get; set; } = null!;

    }
}
