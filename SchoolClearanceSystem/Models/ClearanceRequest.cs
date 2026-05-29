public class ClearanceRequest
{
    public int RequestId { get; set; }
    public string UserID { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
    public string Semester { get; set; }
    public string AcademicYear { get; set; }
    public string DateSubmitted { get; set; }

    // Add these:
    public string FullName { get; set; }
    public string Program { get; set; }
    public string Year { get; set; }
    public string Office { get; set; }
    public string FilePath { get; set; }
    public string DateProcessed { get; set; }
    public string Department { get; set; }
}