using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem.Models
{
    public class ClearanceRecord
    {
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public string Status => "Completed";
    }
}
