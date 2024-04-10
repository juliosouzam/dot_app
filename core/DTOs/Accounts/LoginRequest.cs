using System.ComponentModel.DataAnnotations;

namespace core.DTOs.Accounts
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}