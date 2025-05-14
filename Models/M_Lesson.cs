using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_Lesson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("defaultOrder")]
        public int DefaultOrder { get; set; }

        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("vocabulary_topic")]
        public string? VocabularyTopic { get; set; }

        [JsonPropertyName("grammar_topic")]
        public string? GrammarTopic { get; set; }

        [Required]
        [JsonPropertyName("level")]
        public string Level { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
