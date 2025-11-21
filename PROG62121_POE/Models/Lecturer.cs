using System.Collections.Generic;

namespace PROG62121_POE.Models
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal HourlyRate { get; set; }
        public string Role { get; set; } = "Lecturer";

        // Navigation property for all claims submitted by this lecturer
        public ICollection<Claim> Claims { get; set; }
    }
}
