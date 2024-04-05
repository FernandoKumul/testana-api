using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class AnswersQuestionsUser
{
    public int Id { get; set; }

    public int QuestionAnswerId { get; set; }

    public int UserAnswerId { get; set; }

    public string OtherAnswer { get; set; } = null!;

    public bool? Correct { get; set; }

    public virtual QuestionAnswer QuestionAnswer { get; set; } = null!;

    public virtual UsersAnswer UserAnswer { get; set; } = null!;
}
