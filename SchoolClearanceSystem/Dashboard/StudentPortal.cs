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
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {
        // OOP: Using a Dictionary to encapsulate file states dynamically by Department
        private readonly Dictionary<string, string> _uploadedFiles = new Dictionary<string, string>
        {
            { "SSG", string.Empty }, { "Treasurer", string.Empty }, { "Technical", string.Empty }
        };

        private const string CurrentSemester = "1st Semester";
        private const string CurrentAcademicYear = "2025-2026";

        public StudentPortal()
        {
            InitializeComponent();
            gridControlOfficeStatus.MainView = gridView2;

            // OOP Polymorphism & Service Delegation: Routing click events straight to specialized handlers
            btnUploadSSGRequirement.Click += (s, e) => HandleFileUpload("SSG");
            btnUploadTreasurerRequirement.Click += (s, e) => HandleFileUpload("Treasurer");
            btnViewSSGPhoto.Click += (s, e) => DocumentService.ViewDocument(_uploadedFiles["SSG"]);
            btnViewTreasurerPhoto.Click += (s, e) => DocumentService.ViewDocument(_uploadedFiles["Treasurer"]);

            // Map runtime context profiles
            txtWelcome.Text = Session.CurrentUser != null ? $"Welcome, {Session.CurrentUser.FullName}!" : "Welcome!";
            lblFullName.Text = Session.CurrentUser?.FullName ?? "Unknown User";
            lblUserID.Text = Session.CurrentUser?.UserID?.ToString() ?? "0000";
            lblProgram.Text = Session.CurrentUser?.Program ?? "N/A";

            UpdateDashboard();
            EvaluateSubmissionEligibility();
        }

        private void EvaluateSubmissionEligibility()
        {
            if (Session.CurrentUser == null) return;
            try
            {
                bool isSubmitted = new ClearanceRepository().HasExistingRequest(Session.CurrentUser.UserID.ToString(), CurrentSemester, CurrentAcademicYear);

                // Keep the button text completely static per your preference
                btnSubmitRequest.Text = "Submit Request";

                // Declarative State Management: Only enable controls if NOT submitted
                btnSubmitRequest.Enabled = !isSubmitted;
                btnUploadSSGRequirement.Enabled = btnUploadTreasurerRequirement.Enabled = !isSubmitted;
            }
            catch (Exception ex) { Debug.WriteLine($"Eligibility check crash: {ex.Message}"); }
        }

        #region Navigation and Presentation Layouts

        private void sbDashboard_Click_1(object sender, EventArgs e) => SwitchPage(pageDashboard, true);
        private void sbRequestClearance_Click_1(object sender, EventArgs e) => SwitchPage(pageRequestClearance, false);
        private void sbMyClearance_Click_1(object sender, EventArgs e) => SwitchPage(pageMyClearance, false);

        private void SwitchPage(DevExpress.XtraBars.Navigation.NavigationPage page, bool refreshDashboard)
        {
            naviframeStudent.SelectedPage = page;
            if (refreshDashboard) UpdateDashboard();
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            SwitchPage(pageMyRequest, false);
            if (Session.CurrentUser == null) return;

            try
            {
                // Functional LINQ transformation matching implicit dynamic schemas to standard models
                gridMyRequest.DataSource = new UserRepository().GetStudentStatus(Session.CurrentUser.UserID)
                    .Select(d => new ClearanceStatus { Office = d.Office?.ToString(), Status = d.Status?.ToString(), Remarks = d.Remarks?.ToString() })
                    .ToList();
            }
            catch (Exception ex) { ShowMessage($"Timeline synchronization error: {ex.Message}", true); }
        }

        private void UpdateDashboard()
        {
            if (Session.CurrentUser == null) return;

            UserRepository db = new UserRepository();
            int cleared = db.GetClearedCount(Session.CurrentUser.UserID);
            int percentage = (cleared * 100) / 3;

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            lblPercentage.Text = $"{percentage}%";
            pbOverallProgress.Position = percentage;
            lblStatus.Text = (cleared == 3) ? "Cleared" : "In Progress";
            lblProgress.Text = $"{cleared} out of 3 offices cleared";

            try { gridControlOfficeStatus.DataSource = db.GetStudentStatus(Session.CurrentUser.UserID).ToList(); }
            catch (Exception ex) { XtraMessageBox.Show($"Dashboard grid fetch failed: {ex.Message}"); }
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                bool isApproved = e.CellValue.ToString() == "Approved";
                e.Appearance.ForeColor = isApproved ? Color.ForestGreen : Color.Gray;
                if (isApproved) e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
        }

        #endregion

        #region Dynamic Action Helpers (The Core OOP Changes)

        // OOP Abstraction: Delegates the heavy lifting of UI dialogs to DocumentService
        private void HandleFileUpload(string departmentKey)
        {
            string selectedPath = DocumentService.UploadDocument($"Select {departmentKey} Requirement Attachment");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                _uploadedFiles[departmentKey] = selectedPath;
            }
        }

        private void ShowMessage(string message, bool isError) =>
            XtraMessageBox.Show(message, isError ? "System Alert" : "Success", MessageBoxButtons.OK, isError ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

        #endregion

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                new Login().Show();
                this.Close();
            }
        }

        private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            // Safeguard state assessment validations
            if (string.IsNullOrEmpty(_uploadedFiles["SSG"]) || string.IsNullOrEmpty(_uploadedFiles["Treasurer"]))
            {
                ShowMessage("Please upload all necessary file requirements before submitting.", true);
                return;
            }
            if (Session.CurrentUser == null) return;

            btnSubmitRequest.Enabled = false;

            try
            {
                ClearanceRepository clearanceRepo = new ClearanceRepository();
                string studentId = Session.CurrentUser.UserID.ToString();

                if (clearanceRepo.HasExistingRequest(studentId, CurrentSemester, CurrentAcademicYear))
                {
                    ShowMessage("A request instance already exists in the system database for this active term.", true);
                    EvaluateSubmissionEligibility();
                    return;
                }

                // OOP Data Loop: Committing data transactions dynamically over collection elements instead of repeating blocks
                bool transactionStatus = _uploadedFiles.All(entry =>
                    clearanceRepo.SubmitClearanceRequest(studentId, entry.Key, CurrentSemester, CurrentAcademicYear, entry.Value)
                );

                if (transactionStatus)
                {
                    ShowMessage("Your clearance request has been successfully processed to all three administrative offices!", false);
                    UpdateDashboard();

                    // Clear the dictionary values using memory-safe iteration
                    foreach (var key in _uploadedFiles.Keys.ToList()) _uploadedFiles[key] = string.Empty;

                    EvaluateSubmissionEligibility();
                }
                else
                {
                    ShowMessage("The transaction processing pipeline encountered database synchronization errors.", true);
                    btnSubmitRequest.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Pipeline Crash Fallback Alert: {ex.Message}", true);
                btnSubmitRequest.Enabled = true;
            }
        }
    }
}