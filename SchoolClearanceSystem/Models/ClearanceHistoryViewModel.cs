namespace SchoolClearanceSystem.Models
{
    /// <summary>
    /// Binds directly to your custom DevExpress ListBox item templates.
    /// </summary>
    public class ClearanceHistoryViewModel
    {
        public string PeriodName { get; set; }   // Binds to PeriodName template element
        public string StatusText { get; set; }     // Binds to element2 template element
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
    }
}