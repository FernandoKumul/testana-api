using testana_api.Data.Models;

namespace testana_api.Data.DTOs
{
    public class TestPreviewOutDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string Visibility { get; set; } = null!;

        public string? Image { get; set; }

        public bool Random { get; set; }

        public int? Duration { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Likes { get; set; }

        public bool EvaluateByQuestion { get; set; }

        public int Dislikes { get; set; }

        public virtual UserOutDTO User { get; set; } = null!;

        //public virtual ICollection<UsersAnswer> UsersAnswers { get; set; } = new List<UsersAnswer>();
    }
}
