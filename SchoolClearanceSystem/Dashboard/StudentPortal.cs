using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: SEPARATION OF CONCERNS / STATE RETENTION
    /// This Presentation Layer class orchestrates student interactions. 
    /// It maintains the runtime context of uploaded files through encapsulation fields.
    /// </summary>
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {
        // Encapsulated Class Fields protecting file path states inside this form scope
        private string ssgUploadedFilePath = string.Empty;
        private string treasurerUploadedFilePath = string.Empty;

        // Fields to preserve the active operational clearance period context across the form session
        private string currentSemester = "Not Set";
        private string currentAcademicYear = "Not Set";

        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly UserRepository _userRepo = new UserRepository();

        public StudentPortal()
        {
            InitializeComponent();

            // Map Grid Views to their respective event-driven styling methods
            gridControlOfficeStatus.MainView = gridView2;

            // Connect style events to BOTH grid views
            gridView2.RowCellStyle += ApplyStatusRowStyles;

            // Assuming your gridMyRequest main view is named gridView1
            if (gridMyRequest.MainView is DevExpress.XtraGrid.Views.Grid.GridView gvTimeline)
            {
                gvTimeline.RowCellStyle += ApplyStatusRowStyles;
            }

            // 1. Fetch the True Active Clearance Period Saved by the Admin
            LoadActiveClearancePeriod();

            // 2. Refresh metrics and bind grid data
            UpdateDashboard();

            // Event-Driven Architecture: Wiring event triggers to localized handler methods
            btnUploadSSGRequirement.Click += btnUploadSSGRequirement_Click;
            btnViewSSGPhoto.Click += btnViewSSGRequirement_Click;
            btnUploadTreasurerRequirement.Click += btnUploadTreasurerRequirement_Click;
            btnViewTreasurerPhoto.Click += btnViewTreasurerRequirement_Click;

            // Session Model State Assessment mapping global active context rules
            if (Session.CurrentUser != null)
            {
                txtWelcome.Text = $"Welcome, {Session.CurrentUser.FullName}!";
            }

            lblFullName.Text = Session.CurrentUser?.FullName ?? "Unknown User";
            lblUserID.Text = Session.CurrentUser?.UserID?.ToString() ?? "0000";
            lblProgram.Text = Session.CurrentUser?.Program ?? "N/A";
        }

        /// <summary>
        /// Communicates with SystemRepository to grab the active admin configurations
        /// and applies them fixed/greyed-out onto the UI layout inputs.
        /// </summary>
        private void LoadActiveClearancePeriod()
        {
            try
            {
                // Pull all period configurations recorded in the database architecture
                var periods = _sysRepo.GetAllPeriods();

                // Find the specific period record flagged as currently processing/active (IsActive == 1)
                var activePeriod = periods.FirstOrDefault(p => p.IsActive == 1);

                if (activePeriod != null)
                {
                    currentSemester = activePeriod.Semester?.ToString() ?? "Not Set";
                    currentAcademicYear = activePeriod.AcademicYear?.ToString() ?? "Not Set";
                }
                else
                {
                    XtraMessageBox.Show("Warning: No active clearance period has been opened by the System Administrator.",
                        "System Configuration Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Failed to connect to active operational system variables: {ex.Message}", "Connection Error");
            }

            // Bind the active database values straight to your text inputs
            txtSemester.Text = currentSemester;
            txtCurrentSchoolYear.Text = currentAcademicYear;

            // Enforce Read-Only safety constraints
            txtSemester.ReadOnly = true;
            txtCurrentSchoolYear.ReadOnly = true;

            // Apply consistent modern flat UI visual grey-out stylings
            txtSemester.Properties.Appearance.BackColor = Color.LightGray;
            txtCurrentSchoolYear.Properties.Appearance.BackColor = Color.LightGray;
            txtSemester.Properties.Appearance.ForeColor = Color.DimGray;
            txtCurrentSchoolYear.Properties.Appearance.ForeColor = Color.DimGray;
        }

        #region Navigation and Layout Methods

        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;

            if (Session.CurrentUser != null)
            {
                try
                {
                    // 1. Fetch the raw dynamic list from your database layer
                    var dynamicDataList = _userRepo.GetStudentStatus(Session.CurrentUser.UserID).ToList();

                    // 2. OOP Type Mapping: Explicitly convert dynamic items to ClearanceStatus
                    List<ClearanceStatus> statusRecords = dynamicDataList.Select(d => new ClearanceStatus
                    {
                        Office = d.Office?.ToString(),
                        Status = d.Status?.ToString(),
                        Remarks = d.Remarks?.ToString()
                    }).ToList();

                    // 3. Bind the cleanly typed list to the grid control
                    gridMyRequest.DataSource = statusRecords;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Could not synchronize request timeline history: {ex.Message}",
                        "Sync Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sbMyClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;
        }

        #endregion

        #region Business Logic and Data Processing

        /// <summary>
        /// HOW IT WORKS: Calculates progress metrics and updates UI visual gauges.
        /// Connects to Data Access via UserRepository to query database clearance structures.
        /// </summary>
        private void UpdateDashboard()
        {
            if (Session.CurrentUser == null) return;

            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID);

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            int percentage = (cleared * 100) / 3;
            lblPercentage.Text = $"{percentage}%";

            pbOverallProgress.Position = percentage;
            lblStatus.Text = (cleared == 3) ? "Cleared" : "In Progress";
            lblProgress.Text = $"{cleared} out of 3 offices cleared";

            try
            {
                // Cleanly updates grid using unified database 'Department' layout fields
                var officeData = _userRepo.GetStudentStatus(Session.CurrentUser.UserID).ToList();
                gridControlOfficeStatus.DataSource = officeData;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not load office status data: {ex.Message}");
            }
        }

        /// <summary>
        /// Reusable rendering engine method applied across all data grid collections 
        /// to colorize system status tags dynamically.
        /// </summary>
        private void ApplyStatusRowStyles(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                string status = e.CellValue.ToString().Trim();

                if (status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    e.Appearance.ForeColor = Color.ForestGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                {
                    e.Appearance.ForeColor = Color.DarkOrange;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                }
                else if (status.Equals("On Hold", StringComparison.OrdinalIgnoreCase) ||
                         status.Equals("Declined", StringComparison.OrdinalIgnoreCase) ||
                         status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    e.Appearance.ForeColor = Color.Crimson;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else
                {
                    e.Appearance.ForeColor = Color.Gray;
                }
            }
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e) => ApplyStatusRowStyles(sender, e);

        #endregion

        #region Refactored Clean File Management Abstraction

        private string ExecuteFileSelection()
        {
            using (XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog())
            {
                openFileDialog.Title = "Select a File to Upload";
                openFileDialog.Filter = "Image Files(.BMP;.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (.)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    XtraMessageBox.Show("File successfully selected!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return openFileDialog.FileName;
                }
            }
            return string.Empty;
        }

        private void OpenTargetFile(string targetPath)
        {
            if (string.IsNullOrEmpty(targetPath) || !File.Exists(targetPath))
            {
                XtraMessageBox.Show("No file uploaded yet, or the file no longer exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(targetPath)
                {
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not open the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form UI Interaction Triggers

        private void btnUploadSSGRequirement_Click(object sender, EventArgs e)
        {
            string path = ExecuteFileSelection();
            if (!string.IsNullOrEmpty(path))
            {
                ssgUploadedFilePath = path;
            }
        }

        private void btnViewSSGRequirement_Click(object sender, EventArgs e)
        {
            OpenTargetFile(ssgUploadedFilePath);
        }

        private void btnUploadTreasurerRequirement_Click(object sender, EventArgs e)
        {
            string path = ExecuteFileSelection();
            if (!string.IsNullOrEmpty(path))
            {
                treasurerUploadedFilePath = path;
            }
        }

        private void btnViewTreasurerRequirement_Click(object sender, EventArgs e)
        {
            OpenTargetFile(treasurerUploadedFilePath);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                Login login = new Login();
                login.Show();
                this.Hide();
                this.Close();
            }
        }

        #endregion

        private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ssgUploadedFilePath) || string.IsNullOrEmpty(treasurerUploadedFilePath))
            {
                XtraMessageBox.Show("Please upload all necessary file requirements before submitting your request.",
                    "Incomplete Requirements", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Session.CurrentUser == null)
            {
                XtraMessageBox.Show("Session expired. Please log in again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ClearanceRepository clearanceRepo = new ClearanceRepository();
                string studentId = Session.CurrentUser.UserID.ToString();

                // DYNAMIC SUBMISSION: Uses the exact active period context retrieved from database initialization logs
                bool ssgSubmitted = clearanceRepo.SubmitClearanceRequest(studentId, "SSG", currentSemester, currentAcademicYear, ssgUploadedFilePath);
                bool treasurerSubmitted = clearanceRepo.SubmitClearanceRequest(studentId, "Treasurer", currentSemester, currentAcademicYear, treasurerUploadedFilePath);
                bool technicalSubmitted = clearanceRepo.SubmitClearanceRequest(studentId, "Technical", currentSemester, currentAcademicYear, string.Empty);

                if (ssgSubmitted && treasurerSubmitted && technicalSubmitted)
                {
                    XtraMessageBox.Show("Your clearance request has been submitted successfully to all three offices!",
                        "Submission Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateDashboard();

                    ssgUploadedFilePath = string.Empty;
                    treasurerUploadedFilePath = string.Empty;
                }
                else
                {
                    XtraMessageBox.Show("An error occurred during submission. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Database Submission Failure: {ex.Message}", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}