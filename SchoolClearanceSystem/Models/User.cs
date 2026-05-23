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

        
    }
}