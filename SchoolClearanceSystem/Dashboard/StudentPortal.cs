using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : XtraForm
    {
        // OOP CONCEPT: ENCAPSULATION
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
            gridControlOfficeStatus.MainView = gridView2;
            gridView2.RowCellStyle += ApplyStatusRowStyles;

            // OOP CONCEPT: POLYMORPHISM (Dynamic casting interfaces across different Grid Controls)
            if (gridMyRequest.MainView is GridView gvTimeline) gvTimeline.RowCellStyle += ApplyStatusRowStyles;
            if (gridMyClearance.MainView is GridView gvHistory) gvHistory.RowCellStyle += ApplyStatusRowStyles;

            // Wire UI Interactions
            btnUploadSSGRequirement.Click += btnUploadSSGRequirement_Click;
            btnViewSSGPhoto.Click += btnViewSSGRequirement_Click;
            btnUploadTreasurerRequirement.Click += btnUploadTreasurerRequirement_Click;
            btnViewTreasurerPhoto.Click += btnViewTreasurerRequirement_Click;

            LoadActiveClearancePeriod();
            UpdateDashboard();
            LoadUserSessionContext();
        }

        private void LoadUserSessionContext()
        {
            if (Session.CurrentUser == null) return;
            txtWelcome.Text = "Welcome, " + Session.CurrentUser.FullName + "!";
            lblFullName.Text = Session.CurrentUser.FullName;
            lblUserID.Text = Session.CurrentUser.UserID?.ToString() ?? "0000";
            lblProgram.Text = Session.CurrentUser.Program ?? "N/A";
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
                XtraMessageBox.Show("Database connection error: " + ex.Message, "Connection Error");
            }

            txtSemester.Text = currentSemester;
            txtCurrentSchoolYear.Text = currentAcademicYear;

            // Abstracting visual control formatting setups
            ConfigureReadOnlyTextBox(txtSemester);
            ConfigureReadOnlyTextBox(txtCurrentSchoolYear);
        }

        private void ConfigureReadOnlyTextBox(TextEdit box)
        {
            box.ReadOnly = true;
            box.Properties.Appearance.BackColor = Color.LightGray;
            box.Properties.Appearance.ForeColor = Color.DimGray;
        }

        private void UpdateDashboard()
        {
            if (Session.CurrentUser == null) return;

            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear);
            int percentage = (cleared * 100) / 3;

            lblOfficeCleared.Text = "Offices Cleared: " + cleared + "/3";
            lblPercentage.Text = percentage + "%";
            pbOverallProgress.Position = percentage;

            bool isFullyCleared = (cleared == 3);
            lblStatus.Text = isFullyCleared ? "Cleared" : "In Progress";
            lblStatus.ForeColor = isFullyCleared ? Color.ForestGreen : lblStatus.ForeColor;
            lblProgress.Text = isFullyCleared ? "All 3 offices cleared! Your clearance is complete." : cleared + " out of 3 offices cleared";

            btnSubmitRequest.Enabled = !isFullyCleared;
            btnUploadSSGRequirement.Enabled = !isFullyCleared;
            btnUploadTreasurerRequirement.Enabled = !isFullyCleared;

            if (isFullyCleared)
            {
                btnSubmitRequest.Text = "Clearance Fully Approved";
                btnSubmitRequest.Appearance.BackColor = Color.LightGray;
                btnSubmitRequest.Appearance.ForeColor = Color.DimGray;
                ssgUploadedFilePath = string.Empty;
                treasurerUploadedFilePath = string.Empty;
            }
            else
            {
                btnSubmitRequest.Text = "Submit Request";
            }

            try
            {
                gridControlOfficeStatus.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, currentSemester, currentAcademicYear).ToList();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Could not load office status data: " + ex.Message);
            }
        }

        // OOP CONCEPT: ABSTRACTION (Polymorphic style configuration engine based on key value dictionaries)
        private void ApplyStatusRowStyles(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "Status" || e.CellValue == null) return;
            string status = e.CellValue.ToString().Trim().ToLower();

            if (status == "approved")
            {
                SetRowStyle(e, Color.ForestGreen, FontStyle.Bold);
            }
            else if (status == "pending")
            {
                SetRowStyle(e, Color.DarkOrange, FontStyle.Regular);
            }
            else if (status == "on hold" || status == "declined" || status == "rejected")
            {
                SetRowStyle(e, Color.Crimson, FontStyle.Bold);
            }
            else
            {
                SetRowStyle(e, Color.Gray, FontStyle.Regular);
            }
        }

        private void SetRowStyle(RowCellStyleEventArgs e, Color color, FontStyle style)
        {
            e.Appearance.ForeColor = color;
            e.Appearance.Font = new Font(e.Appearance.Font, style);
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e) => ApplyStatusRowStyles(sender, e);

        // OOP CONCEPT: ABSTRACTION (Encapsulating structural Win32/I/O file processes from core logic)
        #region File Management Abstraction Engine

        private string ExecuteFileSelection()
        {
            using (XtraOpenFileDialog dialog = new XtraOpenFileDialog())
            {
                dialog.Title = "Select a File to Upload";
                dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    XtraMessageBox.Show("File successfully selected!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return dialog.FileName;
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
                XtraMessageBox.Show("Could not launch external file visualization container: " + ex.Message, "Error");
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
            if (Session.CurrentUser != null && _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear) == 3)
            {
                XtraMessageBox.Show("You are already completely cleared for this period! Action blocked.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void BindGridData(NavigationPage targetPage, DevExpress.XtraGrid.GridControl grid, bool forceNull)
        {
            naviframeStudent.SelectedPage = targetPage;
            if (Session.CurrentUser == null || forceNull)
            {
                grid.DataSource = null;
                return;
            }

            grid.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, currentSemester, currentAcademicYear)
                .Select(d => new ClearanceStatus { Office = d.Office?.ToString(), Status = d.Status?.ToString(), Remarks = d.Remarks?.ToString() })
                .ToList();
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            bool alreadyCleared = Session.CurrentUser != null && _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear) == 3;
            BindGridData(pageMyRequest, gridMyRequest, alreadyCleared);
        }

        private void sbMyClearance_Click_1(object sender, EventArgs e)
        {
            bool notClearedYet = Session.CurrentUser == null || _userRepo.GetClearedCount(Session.CurrentUser.UserID, currentSemester, currentAcademicYear) != 3;
            BindGridData(pageMyClearance, gridMyClearance, notClearedYet);
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

            btnSubmitRequest.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                string studentId = Session.CurrentUser.UserID.ToString();
                var existingRequests = _userRepo.GetStudentStatus(studentId, currentSemester, currentAcademicYear);

                if (existingRequests != null && existingRequests.Any())
                {
                    XtraMessageBox.Show("Submission rejected. You have already filed a clearance request for this term.", "Duplicate Submission Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UpdateDashboard();
                    return;
                }

                if (string.IsNullOrEmpty(ssgUploadedFilePath) || string.IsNullOrEmpty(treasurerUploadedFilePath))
                {
                    XtraMessageBox.Show("Please upload all necessary file requirements before submitting.", "Incomplete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnSubmitRequest.Enabled = true;
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
                XtraMessageBox.Show("System Error processing SQL context fields: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSubmitRequest.Enabled = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                UpdateDashboard();
            }
        }

        #endregion
    }
}