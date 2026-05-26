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

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : XtraForm
    {
        // Stores the file paths selected by the student for each office requirement
        private string _ssgFilePath = string.Empty;
        private string _treasurerFilePath = string.Empty;

        // Active clearance period — loaded from DB on startup
        private string _semester = "Not Set";
        private string _academicYear = "Not Set";

        // Total number of offices a student must be cleared by
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
            // Apply color-coded row styles to all status-bearing grids
            gridControlOfficeStatus.MainView = gridView2;
            gridView2.RowCellStyle += ApplyStatusRowStyles;

            if (gridMyRequest.MainView is GridView gvReq) gvReq.RowCellStyle += ApplyStatusRowStyles;
            if (gridMyClearance.MainView is GridView gvHistory) gvHistory.RowCellStyle += ApplyStatusRowStyles;

            // Wire upload/view buttons to DocumentService — keeps file logic out of the form
            btnUploadSSGRequirement.Click += (s, e) => _ssgFilePath = DocumentService.UploadDocument("Upload SSG Requirement");
            btnViewSSGPhoto.Click += (s, e) => DocumentService.ViewDocument(_ssgFilePath);
            btnUploadTreasurerRequirement.Click += (s, e) => _treasurerFilePath = DocumentService.UploadDocument("Upload Treasurer Requirement");
            btnViewTreasurerPhoto.Click += (s, e) => DocumentService.ViewDocument(_treasurerFilePath);

            // Update clearance slip labels when a different period tile is selected
            tileViewMyClearance.FocusedRowChanged += tileViewMyClearance_FocusedRowChanged;

            LoadActiveClearancePeriod();
            UpdateDashboard();
            LoadUserSessionContext();
        }

        // Fills in the sidebar user info labels from the current session
        // Fills in the sidebar user info labels from the current session
        private void LoadUserSessionContext()
        {
            UIHelper.PopulateUserSessionContext(
                lblWelcome,      
                lblFullName,
                lblUserID,
                lblProgram,
                Session.CurrentUser);
        }

        // Fetches the currently active clearance period from the database
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
                    // Reset so submission is blocked
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

            // If no active period, lock everything down and stop
            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                UIHelper.SetActionButtonsAvailability(false, btnSubmitRequest, btnUploadSSGRequirement, btnUploadTreasurerRequirement);
                UIHelper.DisableButtonAsCompleted(btnSubmitRequest, "No Active Period");
                gridControlOfficeStatus.DataSource = null;
                return;
            }

            // Count how many offices have approved this student
            int cleared = _userRepo.GetClearedCount(Session.CurrentUser.UserID, _semester, _academicYear);
            bool fullyCleared = UIHelper.ValidateFullyClearedStatus(cleared, TotalOffices);

            // Update progress indicators
            UIHelper.UpdateProgressIndicators(lblOfficeCleared, lblPercentage, pbOverallProgress, cleared, TotalOffices);
            UIHelper.UpdateClearanceStatus(lblStatus, lblProgress, cleared, TotalOffices);

            // Disable action buttons if already fully cleared
            bool canAct = !fullyCleared;
            UIHelper.SetActionButtonsAvailability(canAct, btnSubmitRequest, btnUploadSSGRequirement, btnUploadTreasurerRequirement);

            if (fullyCleared)
            {
                UIHelper.DisableButtonAsCompleted(btnSubmitRequest);
                _ssgFilePath = _treasurerFilePath = string.Empty;
            }
            else
            {
                UIHelper.ResetButton(btnSubmitRequest);
            }

            try
            {
                // Reload the per-office status grid
                gridControlOfficeStatus.DataSource = _userRepo
                    .GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear).ToList();
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load office status data: {ex.Message}");
            }
        }
        // ── Row Styling ───────────────────────────────────────────────
        // Colors each row's Status cell based on its value
        private void ApplyStatusRowStyles(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName != "Status" || e.CellValue == null) return;

            switch (e.CellValue.ToString().Trim().ToLower())
            {
                case "approved":
                    SetRowStyle(e, Color.ForestGreen, FontStyle.Bold); break;
                case "pending":
                    SetRowStyle(e, Color.DarkOrange, FontStyle.Regular); break;
                case "on hold":
                case "declined":
                case "rejected":
                    SetRowStyle(e, Color.Crimson, FontStyle.Bold); break;
                default:
                    SetRowStyle(e, Color.Gray, FontStyle.Regular); break;
            }
        }

        // Applies a foreground color and font style to a grid cell
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
            LoadActiveClearancePeriod(); // re-check active period on every dashboard visit
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            // Block access to the request page if already fully cleared
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

        // Loads the student's submitted request statuses into the My Request grid
        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;

            if (Session.CurrentUser == null)
            {
                gridMyRequest.DataSource = null;
                return;
            }

            // Shows the 3 office rows with individual statuses
            gridMyRequest.DataSource = _userRepo
                .GetStudentStatus(Session.CurrentUser.UserID, _semester, _academicYear)
                .Select(d => new ClearanceStatus
                {
                    Office = d.Office?.ToString(),
                    Status = d.Status?.ToString(),
                    Remarks = d.Remarks?.ToString()
                }).ToList();
        }
      

        // Updates the clearance slip preview when the student clicks a different period tile
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

            // Reflect the selected period's info on the clearance slip
            UIHelper.PopulateClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued,
                Session.CurrentUser, sem, year);
        }

        // ── Submit Request ────────────────────────────────────────────
        private void btnSubmitRequest_Click_1(object sender, EventArgs e)
        {
            if (!UIHelper.ValidateUserLoggedIn(Session.CurrentUser)) return;

            // Re-check period is still active before allowing submission
            if (_semester == "Not Set" || _academicYear == "Not Set")
            {
                UIHelper.ShowWarning(
                    "No active clearance period. Please wait for the Administrator to open one.",
                    "Period Closed");
                return;
            }

            btnSubmitRequest.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            // ... rest unchanged

            try
            {
                string studentId = Session.CurrentUser.UserID;

                // Block re-submission if a request already exists for this term
                if (_userRepo.GetStudentStatus(studentId, _semester, _academicYear)?.Any() == true)
                {
                    UIHelper.ShowWarning(
                        "You have already filed a clearance request for this term.",
                        "Duplicate Submission");
                    UpdateDashboard();
                    return;
                }

                // Both file uploads are required before submitting
                if (!UIHelper.ValidateRequiredFiles(_ssgFilePath, _treasurerFilePath))
                {
                    UIHelper.ShowWarning("Please upload all required files before submitting.", "Incomplete");
                    btnSubmitRequest.Enabled = true;
                    return;
                }

                // Submit one request row per office — Technical has no file requirement
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
                // Always reset cursor and refresh dashboard regardless of outcome
                this.Cursor = Cursors.Default;
                UpdateDashboard();
            }
        }

        // ── Logout ────────────────────────────────────────────────────
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmation("Are you sure you want to log out?", "Logout") != DialogResult.Yes)
                return;

            // Clear the global session and return to login
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

            // Auto-select first row so slip populates immediately without needing a click
            if (tileViewMyClearance.RowCount > 0)
            {
                tileViewMyClearance.FocusedRowHandle = 0;

                string sem = tileViewMyClearance.GetRowCellValue(0, "Semester")?.ToString();
                string year = tileViewMyClearance.GetRowCellValue(0, "AcademicYear")?.ToString();

                UIHelper.PopulateClearanceSlip(
                    lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued,
                    Session.CurrentUser, sem, year);
            }
            else
            {
                UIHelper.ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
            }
        }
    }
    }

