using System;

namespace SchoolClearanceSystem.Models
{
    public class ClearanceRequest
    {
        // Unique tracking keys
        public int RequestId { get; set; }
        public string UserID { get; set; } // Composition link to the User's UserID

        // Transactional properties
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string DateSubmitted { get; set; }

        // Default Constructor
        public ClearanceRequest() { }
    }
}