
namespace ApplicationCore.DTOs.Recommendation
{
    public class RecommendationDto
    {
        public int Id { get; set; }
        public int CollaboratorId { get; set; }
        public int QuestionId { get; set; }
        public string Note { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}