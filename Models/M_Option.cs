using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_Option
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("question_id")]
        public int QuestionId { get; set; }

        [Required]
        [JsonPropertyName("option_text")]
        public string OptionText { get; set; } = string.Empty;

        [JsonPropertyName("is_correct")]
        public bool IsCorrect { get; set; } = false;
    }
}
