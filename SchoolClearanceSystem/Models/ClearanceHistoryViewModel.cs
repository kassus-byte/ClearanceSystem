namespace SchoolClearanceSystem.Models
{
    /// <summary>
    /// Binds directly to your custom DevExpress ListBox item templates.
    /// </summary>
    public class ClearanceHistoryViewModel
    {
        public User Student { get; set; }  // Binds to the Student property of ClearanceStatus
        public string PeriodName { get; set; }   // Binds to PeriodName template element
        public string StatusText { get; set; }     // Binds to element2 template element
        public string AcademicYear { get; set; }
        public string Semester { get; set; }

        // --- READ-ONLY SHORTCUT PROPERTIES ---
     
        public string UserID => Student?.UserID;
        public string FullName => Student?.FullName;
        public string Program => Student?.Program;
        public string Year => Student?.Year;

        // Parameterless or Empty Constructor
        // Used for general instantiation
        public ClearanceHistoryViewModel()
        {
        }

        // Parameterized constructor to make instantiating this object faster in queries
      
        public ClearanceHistoryViewModel(User student, string periodName, string statusText, string academicYear, string semester)
        {
            Student = student; // Saves the complete user profile details
            PeriodName = periodName;
            StatusText = statusText;
            AcademicYear = academicYear;
            Semester = semester;
        }
    }
}