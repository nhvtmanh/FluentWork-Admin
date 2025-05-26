using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FluentWork_Admin.Models
{
    public class M_User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [JsonProperty("fullname")]
        public string Fullname { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string? Password { get; set; }

        [Required]
        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
