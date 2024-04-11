using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testana_api.Data.Models;

public partial class Question
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

    public virtual ICollection<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();

    public virtual QuestionType QuestionType { get; set; } = null!;

    [JsonIgnore]
    public virtual Test Test { get; set; } = null!;
}
