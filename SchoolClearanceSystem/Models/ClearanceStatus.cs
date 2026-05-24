using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem.Models
{
    public class ClearanceStatus
    {
        //ClearanceStatus shows the user has a profile in the system and has clearance records.
        //It is used to show the current status of the user's clearance process.
        public User Student { get; set; }
        public string Office { get; set; }     
        public string Status { get; set; }    
        public string Remarks { get; set; }

        //A read-only property for fast identity access.
        //It returns the UserID of the associated Student, or null if Student is null.
        public string UserID => Student?.UserID; 
        public string FullName => Student?.FullName;
        public string Program => Student?.Program;
        public string Year => Student?.Year;
        public ClearanceStatus()
        {
        }

        // Parameterized Constructor (For quick manual creation in your backend repo)
        //automatically called during object creation
        public ClearanceStatus(User student, string office, string status, string remarks)
            
        {
            Student = student;
            Office = office;
            Status = status;
            Remarks = remarks;
        }
    }
}

