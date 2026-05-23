namespace SchoolClearanceSystem
{
    public class ClearanceRequest : User
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string DateSubmitted { get; set; }

        // Calls the empty constructor in User.cs
        public ClearanceRequest() : base() { } 
    }
}