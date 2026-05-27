using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using SchoolClearanceSystem.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;

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

            btnUploadSSGRequirement.Click += (s, e) => _ssgFilePath = DocumentService.UploadDocument("Upload SSG Requirement");
            btnViewSSGPhoto.Click += (s, e) => DocumentService.ViewDocument(_ssgFilePath);
            btnUploadTreasurerRequirement.Click += (s, e) => _treasurerFilePath = DocumentService.UploadDocument("Upload Treasurer Requirement");
            btnViewTreasurerPhoto.Click += (s, e) => DocumentService.ViewDocument(_treasurerFilePath);

            tileViewMyClearance.FocusedRowChanged += tileViewMyClearance_FocusedRowChanged;

            LoadActiveClearancePeriod();
            UpdateDashboard();
            LoadUserSessionContext();
        }

        private void LoadUserSessionContext()
        {
            UIHelper.PopulateUserSessionContext(
                lblWelcome,
                lblFullName,
                lblUserID,
                lblProgram,
                Session.CurrentUser);
        }

        private void LoadActiveClearancePeriod()
        {
            try
            {
                var active = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
                if (active != null)
                {
                    _semester = active.Semester ?? "Not Set";
                    _academicYear = active.AcademicYear ?? "Not Set";
                }
                else
                {
                    _semester = "Not Set";
                    _academicYear = "Not Set";
                }
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Database connection error: {ex.Message}", "Connection Error");
                _semester = "Not Set";
                _academicYear = "Not Set";
            }

            UIHelper.SetPeriodFields(txtSemester, txtCurrentSchoolYear, _semester, _academicYear);
        }

        // ── Dashboard ─────────────────────────────────────────────────
        private void UpdateDashboard()
        {
            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser)) return;

            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                btnSubmitRequest.Enabled = false;
                btnSubmitRequest.Text = "No Active Period";
                btnUploadSSGRequirement.Enabled = false;
                btnUploadTreasurerRequirement.Enabled = false;
                gridControlOfficeStatus.DataSource = null;
                return;
            }

            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID, _semester, _academicYear);
            bool fullyCleared = UIHelper.ValidateFullyClearedStatus(cleared, TotalOffices);

            UIHelper.UpdateProgressIndicators(lblOfficeCleared, lblPercentage, pbOverallProgress, cleared, TotalOffices);
            UIHelper.UpdateClearanceStatus(lblStatus, lblProgress, cleared, TotalOffices);

            if (fullyCleared)
            {
                btnSubmitRequest.Enabled = false;
                btnSubmitRequest.Text = "Clearance Fully Approved";
                btnUploadSSGRequirement.Enabled = false;
                btnUploadTreasurerRequirement.Enabled = false;
                _ssgFilePath = _treasurerFilePath = string.Empty;
            }
            else
            {
                btnSubmitRequest.Enabled = true;
                btnSubmitRequest.Text = "Submit Request";
                btnUploadSSGRequirement.Enabled = true;
                btnUploadTreasurerRequirement.Enabled = true;
            }

            try
            {
                gridControlOfficeStatus.DataSource = _userRepo
                    .GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear).ToList();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load office status data: {ex.Message}");
            }
        }

        // ── Row Styling ───────────────────────────────────────────────
        private void ApplyStatusRowStyles(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "Status" || e.CellValue == null) return;

            switch (e.CellValue.ToString().Trim().ToLower())
            {
                case "approved":
                    SetRowStyle(e, Color.ForestGreen, FontStyle.Bold); break;
                case "pending":
                    SetRowStyle(e, Color.DarkRed, FontStyle.Bold); break;
                case "on hold":
                    SetRowStyle(e, Color.DarkOrange, FontStyle.Bold); break;
                case "declined":
                case "rejected":
                    SetRowStyle(e, Color.Crimson, FontStyle.Bold); break;
                default:
                    SetRowStyle(e, Color.Gray, FontStyle.Regular); break;
            }
        }

        private void SetRowStyle(RowCellStyleEventArgs e, Color color, FontStyle style)
        {
            e.Appearance.ForeColor = color;
            e.Appearance.Font = new Font(e.Appearance.Font, style);
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e) =>
            ApplyStatusRowStyles(sender, e);

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
                UIHelper.ValidateFullyClearedStatus(
                    _userRepo.GetClearedCount(Session.CurrentUser.UserID, _semester, _academicYear),
                    TotalOffices))
            {
                UIHelper.ShowWarning("You are already fully cleared for this period.", "Access Denied");
                return;
            }
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;

            if (Session.CurrentUser == null)
            {
                gridMyRequest.DataSource = null;
                return;
            }

            gridMyRequest.DataSource = _userRepo
                .GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear)
                .Select(d => new ClearanceStatus
                {
                    Office = d.Office?.ToString(),
                    Status = d.Status?.ToString(),
                    Remarks = d.Remarks?.ToString()
                }).ToList();
        }

        private void tileViewMyClearance_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int handle = tileViewMyClearance.FocusedRowHandle;
            if (handle < 0)
            {
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                return;
            }

            string sem = tileViewMyClearance.GetRowCellValue(handle, "Semester")?.ToString();
            string year = tileViewMyClearance.GetRowCellValue(handle, "AcademicYear")?.ToString();
            if (string.IsNullOrEmpty(sem) || string.IsNullOrEmpty(year))
            {
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                return;
            }

            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser))
            {
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                return;
            }

            UIHelper.PopulateClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued,
                Session.CurrentUser, sem, year);
        }

        // ── Submit Request ────────────────────────────────────────────
        private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser)) return;

            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                UIHelper.ShowWarning(
                    "No active clearance period. Please wait for the Administrator to open one.",
                    "Period Closed");
                return;
            }

            string studentId = Session.CurrentUser.UserID;

            if (_userRepo.GetStudentStatus(studentId, _semester, _academicYear)?.Any() == true)
            {
                UIHelper.ShowWarning(
                    "You have already filed a clearance request for this term.",
                    "Duplicate Submission");
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
                bool ok =
                    _clearanceRepo.SubmitClearanceRequest(studentId, "SSG", _semester, _academicYear, _ssgFilePath) &&
                    _clearanceRepo.SubmitClearanceRequest(studentId, "Treasurer", _semester, _academicYear, _treasurerFilePath) &&
                    _clearanceRepo.SubmitClearanceRequest(studentId, "Technical", _semester, _academicYear, string.Empty);

                if (ok)
                    UIHelper.ShowSuccess("Clearance request submitted successfully!");
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
            if (UIHelper.ShowConfirmation("Are you sure you want to log out?", "Logout") != DialogResult.Yes)
                return;

            Session.CurrentUser = null;
            new Login().Show();
            this.Close();
        }

        private void sbMyClearance_Click(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;

            if (Session.CurrentUser == null)
            {
                gridMyClearance.DataSource = null;
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                return;
            }

            gridMyClearance.DataSource = _userRepo
                .GetStudentClearancePeriods(Session.CurrentUser.UserID)
                .Select(p => new
                {
                    p.Semester,
                    p.AcademicYear,
                    Completed = "Completed"
                }).ToList();

            tileViewMyClearance.RefreshData();

            if (tileViewMyClearance.RowCount > 0)
            {
                tileViewMyClearance.FocusedRowHandle = 0;

                string sem = tileViewMyClearance.GetRowCellValue(0, "Semester")?.ToString();
                string year = tileViewMyClearance.GetRowCellValue(0, "AcademicYear")?.ToString();

                if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(year))
                {
                    UIHelper.PopulateClearanceSlip(
                        lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued,
                        Session.CurrentUser, sem, year);
                }
                else
                {
                    UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                }
            }
            else
            {
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
            }
        }

        private void btnDownloadClearance_Click(object sender, EventArgs e)
        {
            try
            {
                string currentUserId = Session.CurrentUser?.UserID?.ToString();

                if (string.IsNullOrEmpty(currentUserId))
                {
                    UIHelper.ShowWarning("Active session expired. Please log in again.", "Authentication Warning");
                    return;
                }

                var currentStudent = _userRepo.GetUsersByRole("Student")?
                    .FirstOrDefault(u => u.UserID.ToString() == currentUserId);

                if (currentStudent == null)
                {
                    UIHelper.ShowError("Could not verify account.", "Execution Error");
                    return;
                }

                var tileView = gridMyClearance.MainView as DevExpress.XtraGrid.Views.Tile.TileView;

                if (tileView == null)
                {
                    UIHelper.ShowError("Grid layout configuration error.", "Error");
                    return;
                }

                if (tileView.RowCount == 0)
                {
                    UIHelper.ShowWarning("No clearance records available to download.", "Information");
                    return;
                }

                int rowHandle = tileView.FocusedRowHandle >= 0 ? tileView.FocusedRowHandle : 0;
                dynamic selectedRow = tileView.GetRow(rowHandle);

                if (selectedRow == null)
                {
                    UIHelper.ShowError("Could not read selected row records.", "Execution Error");
                    return;
                }

                string semText = selectedRow.Semester?.ToString() ?? "N/A";
                string syText = selectedRow.AcademicYear?.ToString() ?? "N/A";

                var statuses = _userRepo.GetStudentStatus(currentUserId, semText, syText).ToList();

                string tech = statuses.FirstOrDefault(r => r.Office == "Technical")?.Status ?? "NOT CLEARED";
                string ssg = statuses.FirstOrDefault(r => r.Office == "SSG")?.Status ?? "NOT CLEARED";
                string tres = statuses.FirstOrDefault(r => r.Office == "Treasurer")?.Status ?? "NOT CLEARED";

                var report = new StudentClearanceSlip();
                var studentDataSource = new System.Collections.Generic.List<SchoolClearanceSystem.Models.User> { currentStudent };
                report.InitData(studentDataSource, tech, ssg, tres, semText, syText);

                ReportPrintTool printTool = new ReportPrintTool(report);
                printTool.ShowPreviewDialog();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not construct clearance document layout: {ex.Message}", "Report Engine Error");
            }
        }
    }
}