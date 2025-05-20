using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FluentWork_Admin.Models
{
    public class M_Account_Register
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Fullname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
    public class M_Account_Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
