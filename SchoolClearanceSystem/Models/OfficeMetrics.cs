namespace SchoolClearanceSystem.Models
{
    /// <summary>
    /// OOP CONCEPT: ENCAPSULATION
    /// Data container class that groups and isolates metrics for an individual office desk.
    /// </summary>
    public class OfficeMetrics
    {
        public int ClearedCount { get; set; }
        public int PendingCount { get; set; }
        public int OnHoldCount { get; set; }
        public int TotalStudentsCount { get; set; }

        public OfficeMetrics()
        {
            ClearedCount = 0;
            PendingCount = 0;
            OnHoldCount = 0;
            TotalStudentsCount = 0;
        }
    }
}