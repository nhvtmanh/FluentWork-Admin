using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("fullname")]
        public string Fullname { get; set; } = string.Empty;

        [JsonPropertyName("password_hash")]
        public string? PasswordHash { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("create_at")]
        public DateTime? CreateAt { get; set; }

        [JsonPropertyName("update_at")]
        public DateTime? UpdateAt { get; set; }
    }
}
