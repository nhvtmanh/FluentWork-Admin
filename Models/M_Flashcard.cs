using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_Flashcard
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("word")]
        public string Word { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("definition")]
        public string Definition { get; set; } = string.Empty;
    }
}
