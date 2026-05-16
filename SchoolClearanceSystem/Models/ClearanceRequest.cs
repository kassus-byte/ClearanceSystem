using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem
{
    public class ClearanceRequest : User
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string DateSubmitted { get; set; }

     
        public ClearanceRequest() : base()
        {
        }

      
        public ClearanceRequest(string userId, string fullName, string program, string year, string status, string remarks,
            string semester, string acedemicYear, string dateSubmitted, string attachmentPath = null)
            : base(userId, fullName, program, year)
        {
            Status = status;
            Remarks = remarks;
            Semester = semester;
            AcademicYear = acedemicYear;
            DateSubmitted = dateSubmitted;
        
        }
    }
}