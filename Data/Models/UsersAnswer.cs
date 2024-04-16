using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testana_api.Data.Models;

public partial class UsersAnswer
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int TestId { get; set; }

    public string? Name { get; set; }

    public DateTime? CompletionDate { get; set; }

    public virtual ICollection<AnswersQuestionsUser> AnswersQuestionsUsers { get; set; } = new List<AnswersQuestionsUser>();

    [JsonIgnore]
    public virtual Test Test { get; set; } = null!;

    [JsonIgnore]
    public virtual User? User { get; set; } = null!;
}
