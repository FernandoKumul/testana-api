using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class Recommendation
{
    public int Id { get; set; }

    public int CollaboratorId { get; set; }

    public int QuestionId { get; set; }

    public string Note { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Collaborator Collaborator { get; set; } = null!;
}
