using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class Test
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Visibility { get; set; } = null!;

    public string? Image { get; set; }

    public bool Status { get; set; }

    public bool Random { get; set; }

    public int? Duration { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Likes { get; set; }

    public bool EvaluateByQuestion { get; set; }

    public int Dislikes { get; set; }

    public string Tags { get; set; } = null!;

    public virtual ICollection<Collaborator> Collaborators { get; set; } = new List<Collaborator>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UsersAnswer> UsersAnswers { get; set; } = new List<UsersAnswer>();
}
