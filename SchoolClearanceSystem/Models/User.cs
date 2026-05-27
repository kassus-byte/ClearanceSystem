using System.IO;

namespace SchoolClearanceSystem.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string FullName => string.IsNullOrWhiteSpace(LastName) && string.IsNullOrWhiteSpace(FirstName)
            ? "Unknown"
            : string.IsNullOrWhiteSpace(MiddleName)
                ? $"{LastName}, {FirstName}"
                : $"{LastName}, {FirstName} {MiddleName}";

        public string Program { get; set; }
        public string Year { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string UploadPath { get; set; }
        public string DateCreated { get; set; }

        public User() { }
    }
}