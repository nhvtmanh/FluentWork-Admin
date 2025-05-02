using FluentWork_Admin.Enums;
using System.ComponentModel.DataAnnotations;

namespace FluentWork_Admin.Models
{
    public class M_Question
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? VocabularyTopic { get; set; }
        public string? GrammarTopic { get; set; }
        public string Level { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public List<M_Option> Options { get; set; } = new List<M_Option>();
    }
    public class EM_Question
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = string.Empty;

        public string? VocabularyTopic { get; set; }

        public string? GrammarTopic { get; set; }

        [Required]
        public string Level { get; set; } = string.Empty;

        [Required]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        public string Explanation { get; set; } = string.Empty;

        public List<M_Option> Options { get; set; } = new List<M_Option>();
    }
}
