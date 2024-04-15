
namespace ApplicationCore.DTOs.RecommendationToUpdate
{
    public class RecommendationToUpdateDto
    {
        public int Id { get; set; }
        public string Note { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}