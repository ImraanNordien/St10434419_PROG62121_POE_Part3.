namespace PROG62121_POE.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }              // Unique claim ID
        public int LecturerId { get; set; }           // FK to Lecturer/User
        public string LecturerName { get; set; }      // Display lecturer name
        public int HoursWorked { get; set; }          // Hours worked
        public decimal HourlyRate { get; set; }       // Pulled from Lecturer (set by HR)

        // Auto-calculated total
        public decimal TotalPayment => HoursWorked * HourlyRate;

        public string Notes { get; set; }             // Optional claim notes
        public string FileName { get; set; }          // Supporting document
        public string Status { get; set; } = "Pending"; // Pending, Verified, Approved, Rejected
    }
}
