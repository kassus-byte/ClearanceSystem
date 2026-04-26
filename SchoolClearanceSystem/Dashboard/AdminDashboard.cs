using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        // Global instance of your Database Manager
        DatabaseManager db = new DatabaseManager();

        public AdminDashboard()
        {
            InitializeComponent();

            // Link the SearchControl to the Grid so filtering works automatically
            scStudentFindPanel.Client = gcStudents;

            // Load data for all grids immediately on startup
            RefreshData();
        }

        private void RefreshData()
        {
            // 1. Load Students
            string studentQuery = "SELECT UserID, FullName, Program, Year FROM Users WHERE Role = 'Student' ORDER BY FullName ASC";
            gcStudents.DataSource = db.GetDataTable(studentQuery);

            // 2. Load Office Accounts (Now using the variable!)
            string officeQuery = "SELECT UserID, FullName, Role as 'Designation', Program as 'Department' " +
                                 "FROM Users WHERE Role NOT IN ('Student', 'Admin') ORDER BY Role ASC";

            // Assign the data to your new office grid
            gcOffice.DataSource = db.GetDataTable(officeQuery);
        }

        // --- BUTTON ACTIONS ---

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            // Get the ID from the selected row in the GridView
            object cellValue = gvStudents.GetFocusedRowCellValue("UserID");

            if (cellValue == null)
            {
                XtraMessageBox.Show("Please select a student from the list.", "Selection Required");
                return;
            }

            string selectedID = cellValue.ToString();

            // Ask for confirmation
            if (XtraMessageBox.Show($"Are you sure you want to delete user {selectedID}?", "Confirm Action",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (db.DeleteUser(selectedID))
                {
                    XtraMessageBox.Show("User deleted successfully.");
                    RefreshData(); // Reload the grid to reflect the deletion
                }
            }
        }

        private void btnAddNewStudent_Click(object sender, EventArgs e)
        {
            // Logic to open your Add/Register form
            // When that form closes, call RefreshData() to show the new student
        }

        // --- DATA SYNC (Manual Refresh) ---

        // If you want the grid to refresh whenever the user clicks a specific sidebar item
        private void aceStudentSubItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void accordionControlElement2_Click(object sender, EventArgs e)
        {

        }

        private void accordionControlElement5_Click(object sender, EventArgs e)
        {

        }

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
           
            // Updates the table we just created
            db.ToggleClearanceSeason(tsClearanceStatus.IsOn);

            string status = tsClearanceStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance is now {status}.");
        }
    }
    }
