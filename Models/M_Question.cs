using FluentWork_Admin.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_Question
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("topic")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("vocabulary_topic")]
        public string? VocabularyTopic { get; set; }

        [JsonPropertyName("grammar_topic")]
        public string? GrammarTopic { get; set; }

        [Required]
        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("question_text")]
        public string QuestionText { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("explanation")]
        public string Explanation { get; set; } = string.Empty;

        [JsonPropertyName("options")]
        public List<M_Option> Options { get; set; } = new List<M_Option>();
    }
}
