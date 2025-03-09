using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character."
        )]
        public required string Password { get; set; }
    }
}
