namespace SchoolClearanceSystem.Models
{
    /// <summary>
    /// Binds directly to your custom DevExpress ListBox item templates.
    /// </summary>
    public class ClearanceHistoryViewModel : User
    {
        public User Student { get; set; }  // Binds to the Student property of ClearanceStatus
        public string PeriodName { get; set; }   // Binds to PeriodName template element
        public string StatusText { get; set; }     // Binds to element2 template element
        public string AcademicYear { get; set; }
        public string Semester { get; set; }

        //parameterless or Empty Constructor
        //used for general instantiation
        public string UserId => Student?.UserID; 
        public ClearanceHistoryViewModel()
        {
        }
        // Parameterized constructor to make instantiating this object faster in queries
        public ClearanceHistoryViewModel(string periodName, string statusText, string academicYear, string semester)
            :base() // Call the base User constructor if needed
        {
            PeriodName = periodName;
            StatusText = statusText;
            AcademicYear = academicYear;
            Semester = semester;
        }
    }
}