using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class QuestionAnswer
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public string Text { get; set; } = null!;

    public bool Correct { get; set; }

    public virtual ICollection<AnswersQuestionsUser> AnswersQuestionsUsers { get; set; } = new List<AnswersQuestionsUser>();

    public virtual Question Question { get; set; } = null!;
}
