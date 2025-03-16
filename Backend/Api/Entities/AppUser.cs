using Api.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Api.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public required string KnownAs { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastActive { get; set; }
        public required string  Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interests { get; set; }
        public string? LookingFor { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public List<Photo> Photos { get; set; } = [];

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
