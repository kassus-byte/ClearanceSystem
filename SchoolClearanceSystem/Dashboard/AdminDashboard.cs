using DevExpress.XtraEditors;
using SchoolClearanceSystem.Dashboard;
using System;
using System.Data;
using System.Drawing;
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

            // 1. Setup Tab Titles
            tabNavigationPage1.Caption = "Students";
            tabNavigationPage2.Caption = "Office Accounts";

            // 2. IMPORTANT: Subscribe to the Image Loading event
            // This connects the "UploadPath" text to the "IdPhoto" column
            gvStudents.CustomUnboundColumnData += gvStudents_CustomUnboundColumnData;

            // 3. Load the data into the grids
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                // Load Students - Fetching all relevant columns
                string studentQuery = @"SELECT UserID, FullName, Program, Year, DateCreated, UploadPath 
                                        FROM Users 
                                        WHERE Role = 'Student' 
                                        ORDER BY FullName ASC";

                gcStudents.DataSource = db.GetDataTable(studentQuery);

                // Load Office Accounts - Using aliases for cleaner Grid Mapping
                string officeQuery = @"SELECT UserID, FullName, Role as 'Designation', Program as 'Department', DateCreated 
                                       FROM Users 
                                       WHERE Role NOT IN ('Student', 'Admin') 
                                       ORDER BY Role ASC";

                gcOffice.DataSource = db.GetDataTable(officeQuery);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Error refreshing data: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Converts the text file path in 'UploadPath' into a viewable Image for the Grid
        /// </summary>
        private void gvStudents_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            // Must match the 'FieldName' you set in the Grid Designer for the Photo column
            if (e.Column.FieldName == "IdPhoto" && e.IsGetData)
            {
                DataRowView row = e.Row as DataRowView;
                if (row != null && row["UploadPath"] != DBNull.Value)
                {
                    string filePath = row["UploadPath"].ToString();

                    if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    {
                        try
                        {
                            // Load image from the local path
                            e.Value = Image.FromFile(filePath);
                        }
                        catch
                        {
                            e.Value = null; // Handle corrupt images gracefully
                        }
                    }
                }
            }
        }

        private void tsClearanceSeason_Toggled(object sender, EventArgs e)
        {
            db.ToggleClearanceSeason(tsClearanceSeason.IsOn);

            string status = tsClearanceSeason.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance is now {status}.");
        }

        #region Navigation Logic
        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageDashboard;
        }

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
        }

        private void btnClearanceSeason_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageClearanceSeason;
        }
        #endregion

        private void gcStudents_Click(object sender, EventArgs e)
        {
            // Use this for row selection logic later
        }
    }
}