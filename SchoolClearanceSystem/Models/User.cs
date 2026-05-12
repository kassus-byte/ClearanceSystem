using System;
using System.IO;

namespace SchoolClearanceSystem.Models
{
    /// <summary>
    /// Represents a User in the School Clearance System.
    /// This class uses OOP Encapsulation to handle its own validation logic.
    /// </summary>
    public class User
    {
        // --- DATABASE PROPERTIES ---
        // These match your SQLite columns exactly. Dapper uses these to auto-fill the object.
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string UploadPath { get; set; }
        public string DateCreated { get; set; }

        // --- OOP CONSTRUCTORS ---

        /// <summary>
        /// Essential for Dapper: A parameterless constructor.
        /// Dapper needs this to instantiate the object before mapping database rows.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Overloaded constructor for manual creation (Registration logic).
        /// </summary>
        public User(string id, string name, string role, string pass)
        {
            UserID = id;
            FullName = name;
            Role = role;
            Password = pass;
            DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // --- SMART PROPERTIES (OOP ENCAPSULATION) ---

        // Returns true if the user is a student. 
        // Using StringComparison.OrdinalIgnoreCase makes it "typo-proof" (e.g., 'student' vs 'Student').
        public bool IsStudent => Role?.Equals("Student", StringComparison.OrdinalIgnoreCase) ?? false;

        // Fixes the "Zendaya" issue. Returns false if the role is null, empty, or still set to the default placeholder "Role".
        public bool IsValidRole => !string.IsNullOrWhiteSpace(Role) && !Role.Equals("Role", StringComparison.OrdinalIgnoreCase);

        // A helper for UI labels or headers. Example: "John Doe [2024-0001]"
        public string DisplayName => $"{FullName ?? "Unknown User"} [{UserID ?? "No ID"}]";

        // Safety check for the file system. 
        // This is used by DocumentService to decide if the 'View Image' button should even try to work.
        public bool HasUpload => !string.IsNullOrWhiteSpace(UploadPath) && File.Exists(UploadPath);

        // Provides a fallback for the UI if Program/Year are missing (common for Staff/Admin)
        public string DisplayProgramInfo => IsStudent ? $"{Program} - {Year}" : "N/A (Staff)";
    }
}