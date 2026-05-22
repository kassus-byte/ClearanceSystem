using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: INHERITANCE
    /// StudentPortal inherits base graphical behavior and window control operations from DevExpress.XtraEditors.XtraForm.
    /// </summary>
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {
        // OOP CONCEPT: ENCAPSULATION
        // Restricting fields to 'private' ensures external forms cannot alter sensitive session states directly.
        private string ssgUploadedFilePath = string.Empty;
        private string treasurerUploadedFilePath = string.Empty;
        private string currentSemester = "Not Set";
        private string currentAcademicYear = "Not Set";

        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly UserRepository _userRepo = new UserRepository();

        public StudentPortal()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Grid Configuration
            gridControlOfficeStatus.MainView = gridView2;
            gridView2.RowCellStyle += ApplyStatusRowStyles;

            // OOP CONCEPT: POLYMORPHISM (Safe Downcasting via 'is' operator)
            if (gridMyRequest.MainView is DevExpress.XtraGrid.Views.Grid.GridView gvTimeline)
            {
                gvTimeline.RowCellStyle += ApplyStatusRowStyles;
            }

            if (gridMyClearance.MainView is DevExpress.XtraGrid.Views.Grid.GridView gvHistory)
            {
                gvHistory.RowCellStyle += ApplyStatusRowStyles;
            }

            // Wire UI Control Interaction Event Triggers
            btnUploadSSGRequirement.Click += btnUploadSSGRequirement_Click;
            btnViewSSGPhoto.Click += btnViewSSGRequirement_Click;
            btnUploadTreasurerRequirement.Click += btnUploadTreasurerRequirement_Click;
            btnViewTreasurerPhoto.Click += btnViewTreasurerRequirement_Click;

            // Load and Sync Runtime State Variables
            LoadActiveClearancePeriod();
            UpdateDashboard();
            LoadUserSessionContext();
        }

        private void LoadUserSessionContext()
        {
            if (Session.CurrentUser != null)
            {
                txtWelcome.Text = $"Welcome, {Session.CurrentUser.FullName}!";
                lblFullName.Text = Session.CurrentUser.FullName;
                lblUserID.Text = Session.CurrentUser.UserID?.ToString() ?? "0000";
                lblProgram.Text = Session.CurrentUser.Program ?? "N/A";
            }
        }

        private void LoadActiveClearancePeriod()
        {
            try
            {
                var activePeriod = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);

                if (activePeriod != null)
                {
                    currentSemester = activePeriod.Semester?.ToString() ?? "Not Set";
                    currentAcademicYear = activePeriod.AcademicYear?.ToString() ?? "Not Set";
                }
                else
                {
                    XtraMessageBox.Show("Warning: No active clearance period has been opened by the Administrator.", "System Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Database connection error: {ex.Message}", "Connection Error");
            }

            // Enforce layout property values and style constraints
            txtSemester.Text = currentSemester;
            txtCurrentSchoolYear.Text = currentAcademicYear;
            txtSemester.ReadOnly = true;
            txtCurrentSchoolYear.ReadOnly = true;

            txtSemester.Properties.Appearance.BackColor = Color.LightGray;
            txtCurrentSchoolYear.Properties.Appearance.BackColor = Color.LightGray;
            txtSemester.Properties.Appearance.ForeColor = Color.DimGray;
            txtCurrentSchoolYear.Properties.Appearance.ForeColor = Color.DimGray;
        }

        private void UpdateDashboard()
        {
            if (Session.CurrentUser == null) return;

            // FIXED: Added period context variables to target ONLY the active admin configurations
            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear);
            int percentage = (cleared * 100) / 3;

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            lblPercentage.Text = $"{percentage}%";
            pbOverallProgress.Position = percentage;

            if (cleared == 3)
            {
                lblStatus.Text = "Cleared";
                lblStatus.ForeColor = Color.ForestGreen;
                lblProgress.Text = "All 3 offices cleared! Your clearance is complete.";

                btnSubmitRequest.Enabled = false;
                btnSubmitRequest.Text = "Clearance Fully Approved";
                btnSubmitRequest.Appearance.BackColor = Color.LightGray;
                btnSubmitRequest.Appearance.ForeColor = Color.DimGray;

                btnUploadSSGRequirement.Enabled = false;
                btnUploadTreasurerRequirement.Enabled = false;
                ssgUploadedFilePath = string.Empty;
                treasurerUploadedFilePath = string.Empty;
            }
            else
            {
                lblStatus.Text = "In Progress";
                lblProgress.Text = $"{cleared} out of 3 offices cleared";

                btnSubmitRequest.Enabled = true;
                btnSubmitRequest.Text = "Submit Request";
                btnUploadSSGRequirement.Enabled = true;
                btnUploadTreasurerRequirement.Enabled = true;
            }

            try
            {
                // FIXED: Filtered grid status data dynamically by passing period constraints
                gridControlOfficeStatus.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, currentSemester, currentAcademicYear).ToList();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not load office status data: {ex.Message}");
            }
        }

        // OOP CONCEPT: POLYMORPHISM
        // This event handler dynamically formats cell visual text properties based on runtime row state arguments.
        private void ApplyStatusRowStyles(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                string status = e.CellValue.ToString().Trim().ToLower();

                if (status == "approved")
                {
                    e.Appearance.ForeColor = Color.ForestGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else if (status == "pending")
                {
                    e.Appearance.ForeColor = Color.DarkOrange;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Regular);
                }
                else if (status == "on hold" || status == "declined" || status == "rejected")
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

        // OOP CONCEPT: ABSTRACTION
        // Reusable internal file utility tools. They mask the complex structural operations of open dialog frameworks.
        #region File Management Abstraction Engine

        private string ExecuteFileSelection()
        {
            using (XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog())
            {
                openFileDialog.Title = "Select a File to Upload";
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

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
                XtraMessageBox.Show("No attachment file is currently associated with this request row context.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(targetPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not launch external file visualization container: {ex.Message}", "Error");
            }
        }

        #endregion

        #region UI Event Action Routing

        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            // FIXED: Enforce clearance period isolation inside submission click guards
            if (Session.CurrentUser != null && _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear) == 3)
            {
                XtraMessageBox.Show("You are already completely cleared for this period! Action blocked.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;

            if (Session.CurrentUser != null)
            {
                // FIXED: Isolated current period counting constraints
                int clearedCount = _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear);
                if (clearedCount == 3)
                {
                    gridMyRequest.DataSource = null;
                    return;
                }

                // FIXED: Isolated current timeline metrics using period matching arguments
                gridMyRequest.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, currentSemester, currentAcademicYear)
                    .Select(d => new ClearanceStatus { Office = d.Office?.ToString(), Status = d.Status?.ToString(), Remarks = d.Remarks?.ToString() })
                    .ToList();
            }
        }

        private void sbMyClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;

            // FIXED: Isolated verification history routines using global period parameters
            if (Session.CurrentUser != null && _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear) == 3)
            {
                gridMyClearance.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, currentSemester, currentAcademicYear)
                    .Select(d => new ClearanceStatus { Office = d.Office?.ToString(), Status = d.Status?.ToString(), Remarks = d.Remarks?.ToString() })
                    .ToList();
            }
            else
            {
                gridMyClearance.DataSource = null;
            }
        }

        private void btnUploadSSGRequirement_Click(object sender, EventArgs e) => ssgUploadedFilePath = ExecuteFileSelection();
        private void btnViewSSGRequirement_Click(object sender, EventArgs e) => OpenTargetFile(ssgUploadedFilePath);
        private void btnUploadTreasurerRequirement_Click(object sender, EventArgs e) => treasurerUploadedFilePath = ExecuteFileSelection();
        private void btnViewTreasurerRequirement_Click(object sender, EventArgs e) => OpenTargetFile(treasurerUploadedFilePath);

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to log out?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                new Login().Show();
                this.Close();
            }
        }

       
       private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            if (Session.CurrentUser == null) return;

            // 1. UI DEFENSE: Disable the button immediately to block fast double-clicks
            btnSubmitRequest.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string studentId = Session.CurrentUser.UserID.ToString();

                // 2. DATA DEFENSE: Check if any records exist for this term (regardless of approval status)
                // If GetStudentStatus returns rows, it means they already have pending or processed slots.
                var existingRequests = _userRepo.GetStudentStatus(studentId, currentSemester, currentAcademicYear);

                if (existingRequests != null && existingRequests.Any())
                {
                    XtraMessageBox.Show("Submission rejected. You have already filed a clearance request for this term.",
                                        "Duplicate Submission Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Re-enable UI adjustments based on standard rules
                    UpdateDashboard();
                    return;
                }

                if (string.IsNullOrEmpty(ssgUploadedFilePath) || string.IsNullOrEmpty(treasurerUploadedFilePath))
                {
                    XtraMessageBox.Show("Please upload all necessary file requirements before submitting.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSubmitRequest.Enabled = true; // Re-enable so they can try again after choosing files
                    return;
                }

                ClearanceRepository clearanceRepo = new ClearanceRepository();

                bool ssg = clearanceRepo.SubmitClearanceRequest(studentId, "SSG", currentSemester, currentAcademicYear, ssgUploadedFilePath);
                bool treasurer = clearanceRepo.SubmitClearanceRequest(studentId, "Treasurer", currentSemester, currentAcademicYear, treasurerUploadedFilePath);
                bool technical = clearanceRepo.SubmitClearanceRequest(studentId, "Technical", currentSemester, currentAcademicYear, string.Empty);

                if (ssg && treasurer && technical)
                {
                    XtraMessageBox.Show("Your clearance request has been successfully processed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"System Error processing SQL context fields: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSubmitRequest.Enabled = true; // Re-enable on failure so they aren't completely stuck
            }
            finally
            {
                // Restore mouse pointer look and sync element displays
                this.Cursor = Cursors.Default;
                UpdateDashboard();
            }
        }

        #endregion
    }
}