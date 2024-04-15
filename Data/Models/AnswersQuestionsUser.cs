using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class AnswersQuestionsUser
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public int UserAnswerId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;

    public virtual UsersAnswer UserAnswer { get; set; } = null!;
}
