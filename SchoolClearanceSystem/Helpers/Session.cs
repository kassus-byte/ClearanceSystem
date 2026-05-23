using SchoolClearanceSystem.Models;
namespace SchoolClearanceSystem
{
    public class Session
    {
        // this holds the logged-in student's info globally
        public static User CurrentUser { get; set; } 
    }
}
