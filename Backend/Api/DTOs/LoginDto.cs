using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character."
        )]
        public string Password { get; set; } = string.Empty;
    }
}
