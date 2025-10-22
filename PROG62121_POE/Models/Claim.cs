namespace PROG62121_POE.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }              // Unique ID for the claim
        public int LecturerId { get; set; }           // ID of the lecturer submitting
        public string LecturerName { get; set; }      // Optional, for display
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Notes { get; set; }
        public string FileName { get; set; }          // Name of uploaded file
        public string SupportingDocumentPath { get; set; }   // Full path where file is saved
        public string Status { get; set; } = "Pending";
    }
}
