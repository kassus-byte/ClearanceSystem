namespace SchoolClearanceSystem.Models
{
    public class ClearanceRecord
    {
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string Status => "Completed";
    }
}