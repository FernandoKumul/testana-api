﻿using testana_api.Data.Models;

namespace testana_api.Data.DTOs
{
    public class TestInDTO
    {
        public int UserId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Color { get; set; } = null!;

        public string Visibility { get; set; } = null!;

        public string? Image { get; set; }

        public bool Status { get; set; }

        public bool Random { get; set; }

        public int? Duration { get; set; }

        public bool EvaluateByQuestion { get; set; }

        public string Tags { get; set; } = "";

        //public virtual ICollection<Collaborator> Collaborators { get; set; } = new List<Collaborator>();

        public List<QuestionInDTO> Questions { get; set; } = new List<QuestionInDTO>();

    }
}
