using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class UsersAnswer
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TestId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CompletionDate { get; set; }

    public virtual ICollection<AnswersQuestionsUser> AnswersQuestionsUsers { get; set; } = new List<AnswersQuestionsUser>();

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
