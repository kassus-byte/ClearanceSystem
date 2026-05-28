using System;

namespace SchoolClearanceSystem.Models
{
    public class ClearanceRequest
    {
        public int RequestId { get; set; }
        public string UserID { get; set; }
        public string Office { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string DateSubmitted { get; set; }
        public string DateProcessed { get; set; }

        public ClearanceRequest() { }
    }
}