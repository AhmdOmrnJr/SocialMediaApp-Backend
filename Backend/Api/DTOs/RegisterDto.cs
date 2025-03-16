using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character."
        )]
        public string Password { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }
}
