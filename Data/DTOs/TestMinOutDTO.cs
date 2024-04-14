namespace testana_api.Data.DTOs
{
    public class TestMinOutDTO
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int? UsersAnswerId { get; set; }

        public string AuthorName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string Visibility { get; set; } = null!;

        public string? Image { get; set; }

        public bool Status { get; set; }

        public DateTime? CompletionDate { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
