using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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

        public StudentPortal()
        {
            InitializeComponent();
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

            UserRepository db = new UserRepository();
            int cleared = db.GetClearedCount(Session.CurrentUser.UserID);

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            int percentage = (cleared * 100) / 3;
            lblPercentage.Text = $"{percentage}%";

            pbOverallProgress.Position = percentage;
            lblStatus.Text = (cleared == 3) ? "Cleared" : "In Progress";
            lblProgress.Text = $"{cleared} out of 3 offices cleared";

            RefreshOfficeStatus();
        }

        private void RefreshOfficeStatus()
        {
            if (Session.CurrentUser == null) return;

            UserRepository repo = new UserRepository();
            var statusList = repo.GetStudentStatus(Session.CurrentUser.UserID);

            gridControlOfficeStatus.DataSource = statusList;
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                string status = e.CellValue.ToString();
                if (status == "Approved")
                {
                    e.Appearance.ForeColor = Color.ForestGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else if (status == "Pending")
                {
                    e.Appearance.ForeColor = Color.Gray;
                }
            }
        }

        #endregion

        #region Refactored Clean File Management Abstraction

        /// <summary>
        /// OOP CONCEPT: POLYMORPHIC CODE REUSABILITY / FUNCTION ABSTRACTION
        /// Instead of copying and pasting the dialog generation routine across multiple buttons, 
        /// this unified, centralized utility abstracts the common routine away into a single point of control.
        /// </summary>
        private string ExecuteFileSelection()
        {
            using (XtraOpenFileDialog openFileDialog = new XtraOpenFileDialog())
            {
                openFileDialog.Title = "Select a File to Upload";
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    XtraMessageBox.Show("File successfully selected!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return openFileDialog.FileName;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// OOP CONCEPT: POLISHED ABSTRACTION VIA SHELL OS EXECUTION
        /// This private execution helper encapsulates Windows native Process utilities to view items.
        /// </summary>
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
                    UseShellExecute = true // Required in standard modern .NET environments
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
                Session.CurrentUser = null; // Clear static state reference pointers safely
                Login login = new Login();
                login.Show();
                this.Hide();
                this.Close();
            }
        }

        #endregion

        /// <summary>
        /// HOW IT WORKS (Data Transaction Routing Pipeline):
        /// 1. Enforces data entry logic validations to ensure all requirements are satisfied.
        /// 2. Instantiates data layer objects to pack parameter models securely.
        /// 3. Commits transaction data records to SQLite, where BaseOfficeForm forms fetch it.
        /// </summary>
        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            // 1. Validation Check: Ensure files are selected for SSG and Treasurer
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

            string localSemester = "1st Semester";
            string localAcademicYear = "2025-2026";

            try
            {
                ClearanceRepository clearanceRepo = new ClearanceRepository();
                string studentId = Session.CurrentUser.UserID.ToString();

                // 2. Commit transaction rows for each office department securely.
                bool ssgSubmitted = clearanceRepo.SubmitClearanceRequest(
                    studentId,
                    "SSG",
                    localSemester,
                    localAcademicYear,
                    ssgUploadedFilePath
                );

                bool treasurerSubmitted = clearanceRepo.SubmitClearanceRequest(
                    studentId,
                    "Treasurer",
                    localSemester,
                    localAcademicYear,
                    treasurerUploadedFilePath
                );

                // Technical Office requires no uploaded attachment files, passing an empty string or "N/A" reference pointer
                bool technicalSubmitted = clearanceRepo.SubmitClearanceRequest(
                    studentId,
                    "Technical",
                    localSemester,
                    localAcademicYear,
                    string.Empty
                );

                if (ssgSubmitted && treasurerSubmitted && technicalSubmitted)
                {
                    XtraMessageBox.Show("Your clearance request has been submitted successfully to all three offices!",
                        "Submission Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 3. Update the local UI tracking layouts to display state progress changes instantly
                    UpdateDashboard();

                    // Flush path memory tracks upon complete transaction execution
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