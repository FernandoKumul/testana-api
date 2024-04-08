﻿using System.ComponentModel.DataAnnotations;

namespace testana_api.Data.DTOs
{
    public class TestInUpdateDTO
    {
        [Required(ErrorMessage = "El titulo de la pregunta es obligatoria")]
        public string Title { get; set; } = null!;
        
        [Required(ErrorMessage = "La descripción de la pregunta es obligatoria")]
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

        public List<QuestionInUpdateDTO> Questions { get; set; } = new List<QuestionInUpdateDTO>();
    }
}
