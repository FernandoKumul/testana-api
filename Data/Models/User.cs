using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace testana_api.Data.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Avatar { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<Collaborator> Collaborators { get; set; } = new List<Collaborator>();
    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
    [JsonIgnore]
    public virtual ICollection<UsersAnswer> UsersAnswers { get; set; } = new List<UsersAnswer>();
}
