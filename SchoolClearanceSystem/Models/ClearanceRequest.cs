using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem
{
    public class ClearanceRequest 
    {

        //User = using user class as a custom data type
        //holds a complete User (studeent) objects
        public User Student { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string DateSubmitted { get; set; }
        //=======
        //A read-only property for fast identity access.

        public string UserID => Student?.UserID;
        public string FullName => Student?.FullName;
        public string Program => Student?.Program;
        public string Year => Student?.Year;
        public ClearanceRequest() 
        {
        }

      
        public ClearanceRequest(User student, string status, string remarks, string semester, string acedemicYear, string dateSubmitted)
             
        {
            Student = student;
            Status = status;
            Remarks = remarks;
            Semester = semester;
            AcademicYear = acedemicYear;
            DateSubmitted = dateSubmitted;
        
        }
    }
}