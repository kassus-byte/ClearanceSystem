using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem
{
    public class User
    {
        // Properties represent the data (Encapsulation)
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }

        public string UploadPath { get; set; }

        // 1. ADDED: Empty Constructor
        // This allows you to use the "return new User { ... }" syntax in DatabaseManager
        public User()
        {
        }

        // 2. Existing Constructor
        // This allows you to create a user with all data in one line
        public User(string id, string name, string prog, string year, string role, string pass, string path)
        {
            UserID = id;
            FullName = name;
            Program = prog;
            Year = year;
            Role = role;
            Password = pass;
            UploadPath = path; // Add this
        }
    }
}