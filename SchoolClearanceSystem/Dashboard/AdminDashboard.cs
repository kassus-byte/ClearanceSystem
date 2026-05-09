using DevExpress.XtraEditors;
using SchoolClearanceSystem.Dashboard;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        // Global instance of your Database Manager
       // DatabaseManager db = new DatabaseManager();

        public AdminDashboard()
        {
            InitializeComponent();
       //     RefreshData();
           

        }

        private void pageAccountManagement_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void RefreshData()
        //{
        //    // 1. Load Students
        //    string studentQuery = "SELECT UserID, FullName, Program, Year FROM Users WHERE Role = 'Student' ORDER BY FullName ASC";
        //    gcStudents.DataSource = db.GetDataTable(studentQuery);

        //    // 2. Load Office Accounts (Now using the variable!)
        //    string officeQuery = "SELECT UserID, FullName, Role as 'Designation', Program as 'Department' " +
        //                         "FROM Users WHERE Role NOT IN ('Student', 'Admin') ORDER BY Role ASC";

        //    // Assign the data to your new office grid
        //    gcOffice.DataSource = db.GetDataTable(officeQuery);
        //}

        // private void btnDashboard_Click(object sender, EventArgs e)
        //{
        //    // pgDashboard is your Navigation Frame
        //    // The syntax is: [FrameName].SelectedPage = [PageName];
        //    mainNavigationFrame.SelectedPage = pageDashboard;
        //}

        //private void btnAccountManagement_Click(object sender, EventArgs e)
        //{
        //    mainNavigationFrame.SelectedPage = pageAccountManagement;


        //}


        //private void btnClearanceSeason_Click(object sender, EventArgs e)
        //{
        //    mainNavigationFrame.SelectedPage = pageClearanceSeason;
        //}

        //private void tsClearanceSeason_Toggled(object sender, EventArgs e)
        //{

        //    // Updates the table we just created
        //    db.ToggleClearanceSeason(tsClearanceSeason.IsOn);

        //    string status = tsClearanceSeason.IsOn ? "OPEN" : "CLOSED";
        //    XtraMessageBox.Show($"Clearance is now {status}.");

        //}


    }
    }

