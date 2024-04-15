using System.ComponentModel.DataAnnotations;

namespace testana_api.Data.DTOs
{
    public class UserOutDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
