using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testana_api.Data.Models;

public partial class QuestionAnswer
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string Text { get; set; } = null!;

    public bool Correct { get; set; }

    [JsonIgnore]
    public virtual ICollection<AnswersQuestionsUser> AnswersQuestionsUsers { get; set; } = new List<AnswersQuestionsUser>();

    [JsonIgnore]
    public virtual Question Question { get; set; } = null!;
}
