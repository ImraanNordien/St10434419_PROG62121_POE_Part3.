namespace PROG62121_POE.Models
{
    public class User
    {
        public int Id { get; set; }                // Primary key
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";     // Lecturer, ProgrammeCoordinator, AcademicManager, HR
        public decimal HourlyRate { get; set; }    // Only used for Lecturers
    }
}
