using System;
using System.Collections.Generic;

namespace testana_api.Data.Models;

public partial class Collaborator
{
    public int Id { get; set; }

    public int TestId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

    public virtual Test Test { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
