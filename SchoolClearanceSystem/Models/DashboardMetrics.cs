namespace SchoolClearanceSystem.Models
{
    public class DashboardMetrics
    {
        public int TotalStudents { get; set; }
        public int TotalOfficeAccounts { get; set; }
        public int NewRegistrationsCount { get; set; }

        public DashboardMetrics()
        {
            TotalStudents = 0;
            TotalOfficeAccounts = 0;
            NewRegistrationsCount = 0;
        }
    }
}