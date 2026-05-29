using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem.Models
{
    public class ClearancePeriodItem
    {
        public int PeriodID { get; set; }
        public string Semester { get; set; }
        public string AcademicYear { get; set; }
        public int IsActive { get; set; }
    }

}
