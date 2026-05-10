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
            // GRID 1: Only show Students
            string studentQuery = "SELECT UserID, FullName, Role, Program, Year, DateCreated " +
                                  "FROM Users WHERE Role = 'Student' ORDER BY FullName ASC";
            gcStudents.DataSource = db.GetDataTable(studentQuery);

            // GRID 2: Show EVERYTHING ELSE (except the Admin itself)
            // This will include Library, Finance, Registrar, etc.
            string officeQuery = "SELECT UserID, FullName, Role, Program, DateCreated " +
                                 "FROM Users WHERE Role != 'Student' AND Role != 'Admin' " +
                                 "ORDER BY Role ASC";

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

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            // Create form in Register mode with no data
            UserInfoForm frm = new UserInfoForm(FormMode.Register, null);

            // Set pop-up properties
            frm.StartPosition = FormStartPosition.CenterParent;

            // ShowDialog pauses this code. If the user clicks "Save", it returns OK.
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                RefreshData(); // Reload the grids to show the new student
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            // Get the selected row from the GridView (gvStudents)
            DataRowView selectedRow = gvStudents.GetFocusedRow() as DataRowView;

            if (selectedRow != null)
            {
                // Create form in Edit mode and pass the selected row data
                UserInfoForm frm = new UserInfoForm(FormMode.Edit, selectedRow);

                frm.StartPosition = FormStartPosition.CenterParent;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData(); // Reload the grids to show updated info
                }
            }
            else
            {
                XtraMessageBox.Show("Please select a student from the list first.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}