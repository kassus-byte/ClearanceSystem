using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.XtraReports.UI;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using SchoolClearanceSystem.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : XtraForm
    {
        private string _ssgFilePath = string.Empty;
        private string _treasurerFilePath = string.Empty;
        private string _semester = "Not Set";
        private string _academicYear = "Not Set";
        private const int TotalOffices = 3;

        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public StudentPortal()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        // ── Init ──────────────────────────────────────────────────────
        private void InitializeCustomComponents()
        {
            gridControlOfficeStatus.MainView = gridView2;
            gridView2.RowCellStyle += ApplyStatusRowStyles;

            if (gridMyRequest.MainView is GridView gvReq) gvReq.RowCellStyle += ApplyStatusRowStyles;
            if (gridMyClearance.MainView is GridView gvHistory) gvHistory.RowCellStyle += ApplyStatusRowStyles;

            // OOP REUSE: Functional binding pattern compatible with C# 7.3
            ConfigureFileField(txtSSGFilePath, btnUploadSSGRequirement, "Upload SSG Requirement", path => _ssgFilePath = path, () => _ssgFilePath);
            ConfigureFileField(txtTreasurerFilePath, btnUploadTreasurerRequirement, "Upload Treasurer Requirement", path => _treasurerFilePath = path, () => _treasurerFilePath);

            tileViewMyClearance.FocusedRowChanged += tileViewMyClearance_FocusedRowChanged;

            LoadActiveClearancePeriod();
            UpdateDashboard();
            LoadUserSessionContext();
        }

        // Helper pattern wraps duplicate document click/upload handlers 
        private void ConfigureFileField(TextEdit textEdit, SimpleButton uploadBtn, string prompt, Action<string> pathSetter, Func<string> pathGetter)
        {
            textEdit.Properties.ReadOnly = true;
            textEdit.Cursor = Cursors.Hand;
            uploadBtn.Click += (s, e) =>
            {
                string path = DocumentService.UploadDocument(prompt);
                if (!string.IsNullOrEmpty(path))
                {
                    pathSetter(path);
                    textEdit.Text = path;
                }
            };
            textEdit.Click += (s, e) => { if (!string.IsNullOrEmpty(pathGetter())) DocumentService.ViewDocument(pathGetter()); };
        }

        private void LoadUserSessionContext() =>
            UIHelper.PopulateUserSessionContext(lblWelcome, lblFullName, lblUserID, lblProgram, lblYear, Session.CurrentUser);

        private void LoadActiveClearancePeriod()
        {
            try
            {
                var active = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
                _semester = active != null ? active.Semester ?? "Not Set" : "Not Set";
                _academicYear = active != null ? active.AcademicYear ?? "Not Set" : "Not Set";
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Database connection error: {ex.Message}", "Connection Error");
                _semester = _academicYear = "Not Set";
            }
            UIHelper.SetPeriodFields(txtSemester, txtCurrentSchoolYear, _semester, _academicYear);
        }

        // ── Dashboard ─────────────────────────────────────────────────
        private void UpdateDashboard()
        {
            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser)) return;

            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                ToggleSubmissionState(false, "No Active Period");
                gridControlOfficeStatus.DataSource = null;
                return;
            }

            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID, _semester, _academicYear);
            bool fullyCleared = UIHelper.ValidateFullyClearedStatus(cleared, TotalOffices);

            UIHelper.UpdateProgressIndicators(lblOfficeCleared, lblPercentage, pbOverallProgress, cleared, TotalOffices);
            UIHelper.UpdateClearanceStatus(lblStatus, lblProgress, cleared, TotalOffices);
            ToggleSubmissionState(!fullyCleared, fullyCleared ? "Clearance Fully Approved" : "Submit Request");

            if (fullyCleared) _ssgFilePath = _treasurerFilePath = string.Empty;

            try
            {
                gridControlOfficeStatus.DataSource = _userRepo.GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear).ToList();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load office status data: {ex.Message}");
            }
        }

        private void ToggleSubmissionState(bool enabled, string buttonText)
        {
            btnSubmitRequest.Enabled = btnUploadSSGRequirement.Enabled = btnUploadTreasurerRequirement.Enabled = enabled;
            btnSubmitRequest.Text = buttonText;
        }

        // ── Row Styling ───────────────────────────────────────────────
        private void ApplyStatusRowStyles(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "Status" || e.CellValue == null) return;

            Color color;
            FontStyle style;

            // Reverted back to classic C# 7.3 traditional switch statement block
            switch (e.CellValue.ToString().Trim().ToLower())
            {
                case "approved":
                    color = Color.ForestGreen; style = FontStyle.Bold; break;
                case "pending":
                    color = Color.DarkRed; style = FontStyle.Bold; break;
                case "on hold":
                    color = Color.DarkOrange; style = FontStyle.Bold; break;
                case "declined":
                case "rejected":
                    color = Color.Crimson; style = FontStyle.Bold; break;
                default:
                    color = Color.Gray; style = FontStyle.Regular; break;
            }

            e.Appearance.ForeColor = color;
            e.Appearance.Font = new Font(e.Appearance.Font, style);
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e) => ApplyStatusRowStyles(sender, e);

        // ── Navigation ────────────────────────────────────────────────
        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
            LoadActiveClearancePeriod();
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            if (UIHelper.ValidateUserLoggedIn(Session.CurrentUser) &&
                UIHelper.ValidateFullyClearedStatus(_userRepo.GetClearedCount(Session.CurrentUser.UserID, _semester, _academicYear), TotalOffices))
            {
                UIHelper.ShowWarning("You are already fully cleared for this period.", "Access Denied");
                return;
            }
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;
            gridMyRequest.DataSource = Session.CurrentUser == null ? null : _userRepo.GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear)
                .Select(d => new ClearanceStatus { Office = d.Office?.ToString(), Status = d.Status?.ToString(), Remarks = d.Remarks?.ToString() }).ToList();
        }

        private void tileViewMyClearance_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int handle = tileViewMyClearance.FocusedRowHandle;
            if (handle < 0 || Session.CurrentUser == null) { UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued); return; }

            string sem = tileViewMyClearance.GetRowCellValue(handle, "Semester")?.ToString();
            string year = tileViewMyClearance.GetRowCellValue(handle, "AcademicYear")?.ToString();

            if (string.IsNullOrEmpty(sem) || string.IsNullOrEmpty(year))
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
            else
                UIHelper.PopulateClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued, Session.CurrentUser, sem, year);
        }

        // ── Submit Request ────────────────────────────────────────────
        private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser)) return;

            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                UIHelper.ShowWarning("No active clearance period. Please wait for the Administrator to open one.", "Period Closed");
                return;
            }

            string studentId = Session.CurrentUser.UserID;
            if (_userRepo.GetStudentStatus(studentId, _semester, _academicYear)?.Any() == true)
            {
                UIHelper.ShowWarning("You have already filed a clearance request for this term.", "Duplicate Submission");
                return;
            }

            if (!UIHelper.ValidateRequiredFiles(_ssgFilePath, _treasurerFilePath))
            {
                UIHelper.ShowWarning("Please upload all required files before submitting.", "Incomplete");
                return;
            }

            btnSubmitRequest.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (_clearanceRepo.SubmitClearanceRequest(studentId, "SSG", _semester, _academicYear, _ssgFilePath) &&
                    _clearanceRepo.SubmitClearanceRequest(studentId, "Treasurer", _semester, _academicYear, _treasurerFilePath) &&
                    _clearanceRepo.SubmitClearanceRequest(studentId, "Technical", _semester, _academicYear, string.Empty))
                {
                    UIHelper.ShowSuccess("Clearance request submitted successfully!");
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Error processing request: {ex.Message}", "Database Error");
                btnSubmitRequest.Enabled = true;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                UpdateDashboard();
            }
        }

        // ── Logout ────────────────────────────────────────────────────
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmation("Are you sure you want to log out?", "Logout") != DialogResult.Yes) return;
            Session.CurrentUser = null;
            new Login().Show();
            this.Close();
        }

        private void sbMyClearance_Click(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;
            if (Session.CurrentUser == null) { gridMyClearance.DataSource = null; UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued); return; }

            gridMyClearance.DataSource = _userRepo.GetStudentClearancePeriods(Session.CurrentUser.UserID).Select(p => new { p.Semester, p.AcademicYear, Completed = "Completed" }).ToList();
            tileViewMyClearance.RefreshData();

            if (tileViewMyClearance.RowCount > 0)
            {
                tileViewMyClearance.FocusedRowHandle = 0;
                string sem = tileViewMyClearance.GetRowCellValue(0, "Semester")?.ToString();
                string year = tileViewMyClearance.GetRowCellValue(0, "AcademicYear")?.ToString();

                if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(year))
                    UIHelper.PopulateClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued, Session.CurrentUser, sem, year);
                else
                    UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
            }
            else UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
        }

        private void btnDownloadClearance_Click(object sender, EventArgs e)
        {
            try
            {
                string currentUserId = Session.CurrentUser != null ? Session.CurrentUser.UserID?.ToString() : null;
                if (string.IsNullOrEmpty(currentUserId)) { UIHelper.ShowWarning("Active session expired. Please log in again.", "Authentication Warning"); return; }

                var currentStudent = _userRepo.GetUsersByRole("Student")?.FirstOrDefault(u => u.UserID.ToString() == currentUserId);
                var tileView = gridMyClearance.MainView as TileView;

                if (currentStudent == null || tileView == null) { UIHelper.ShowError(tileView == null ? "Grid layout configuration error." : "Could not verify account.", "Error"); return; }
                if (tileView.RowCount == 0) { UIHelper.ShowWarning("No clearance records available to download.", "Information"); return; }

                dynamic selectedRow = tileView.GetRow(tileView.FocusedRowHandle >= 0 ? tileView.FocusedRowHandle : 0);
                if (selectedRow == null) { UIHelper.ShowError("Could not read selected row records.", "Execution Error"); return; }

                string semText = selectedRow.Semester?.ToString() ?? "N/A";
                string syText = selectedRow.AcademicYear?.ToString() ?? "N/A";
                var statuses = _userRepo.GetStudentStatus(currentUserId, semText, syText).ToList();

                var report = new StudentClearanceSlip();
                report.InitData(new List<User> { currentStudent },
                    statuses.FirstOrDefault(r => r.Office == "Technical") != null ? statuses.FirstOrDefault(r => r.Office == "Technical").Status : "NOT CLEARED",
                    statuses.FirstOrDefault(r => r.Office == "SSG") != null ? statuses.FirstOrDefault(r => r.Office == "SSG").Status : "NOT CLEARED",
                    statuses.FirstOrDefault(r => r.Office == "Treasurer") != null ? statuses.FirstOrDefault(r => r.Office == "Treasurer").Status : "NOT CLEARED", semText, syText);

                new ReportPrintTool(report).ShowPreviewDialog();
            }
            catch (Exception ex) { UIHelper.ShowError($"Could not construct clearance document layout: {ex.Message}", "Report Engine Error"); }
        }
    }
}