using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    /// <summary>
    /// OOP CONCEPT: FORM INHERITANCE (Polymorphism)
    /// By changing 'XtraForm' to 'BaseOfficeForm', this dashboard inherits all
    /// the dynamic grid population, searching, filtering, and data updating pipelines automatically.
    /// </summary>
    public partial class SSGOffice : BaseOfficeForm
    {
        public SSGOffice()
        {
            InitializeComponent();

            // OOP Encapsulation: Identify this specific window instance context as the SSG desk.
            // This string feeds directly into the Base Form's data filtration systems.
            this.OfficeName = "SSG";

            // Update the window display title at runtime
            this.Text = "SSG Department Desk - Clearance Management System";
        }
    }
}