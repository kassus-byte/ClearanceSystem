using System;

namespace SchoolClearanceSystem
{
    public class User
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string UploadPath { get; set; }

        // 1. ADD THIS PROPERTY
        public string DateCreated { get; set; }

        public User()
        {
        }

        // 2. UPDATE THIS CONSTRUCTOR
        public User(string id, string name, string prog, string year, string role, string pass, string path, string dateCreated)
        {
            UserID = id;
            FullName = name;
            Program = prog;
            Year = year;
            Role = role;
            Password = pass;
            UploadPath = path;
            DateCreated = dateCreated; // Set it here
        }
    }
}