using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SchoolClearanceSystem.Dashboard;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
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
            RefreshData();
        }

        private void RefreshData()
        {
            // 1. Load Students 
            // UPDATED: Added DateCreated to the SELECT statement
            string studentQuery = "SELECT UserID, FullName, Program, Year, UploadPath, DateCreated FROM Users WHERE Role = 'Student' ORDER BY FullName ASC";
            gcStudents.DataSource = db.GetDataTable(studentQuery);

            // 2. Load Office Accounts
            // UPDATED: Added DateCreated here as well
            string officeQuery = "SELECT UserID, FullName, Role as 'Designation', Program as 'Department', DateCreated " +
                                 "FROM Users WHERE Role NOT IN ('Student', 'Admin') ORDER BY Role ASC";

            gcOffice.DataSource = db.GetDataTable(officeQuery);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            object cellValue = gvStudents.GetFocusedRowCellValue("UploadPath");

            if (cellValue != null && cellValue != DBNull.Value)
            {
                string filePath = cellValue.ToString();

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show($"Could not open image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    XtraMessageBox.Show("The file does not exist at path: " + filePath, "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                XtraMessageBox.Show("No image path found for this student.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

       

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageDashboard;
        }

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
        }

        private void tsStatus_Toggled(object sender, EventArgs e)
        {

            db.ToggleClearanceSeason(tsStatus.IsOn);
            string status = tsStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance is now {status}.");
        }
    }
}