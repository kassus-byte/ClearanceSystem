using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem
{
    public class Session
    {
        // This will hold the logged-in student's info globally
        public static User CurrentUser { get; set; } 
    }
}
