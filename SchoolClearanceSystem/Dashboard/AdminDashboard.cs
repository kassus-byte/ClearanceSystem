using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public AdminDashboard()
        {
            InitializeComponent();

            comboSemester.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            comboAcademicYear.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            gcStudents.MouseDown += (s, e) => EvaluateHitInfo(gvStudents, e.Location);
            gcOffice.MouseDown += (s, e) => EvaluateHitInfo(gvOffice, e.Location);
            txtSearch.TextChanged += (s, e) => ApplyUnifiedFilter();

            RefreshData();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            SetupIdentity();
        }

        // ── Search ────────────────────────────────────────────────────
        private void ApplyUnifiedFilter()
        {
            try
            {
                var view = ActiveView;
                if (view == null) return;

                string search = txtSearch.Text.Trim();
                view.ApplyFindFilter(search);
            }
            catch (Exception ex)
            {
                Notify($"Could not apply filter: {ex.Message}", "Filter Error", MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e) => ApplyUnifiedFilter();

        // ── Navigation ────────────────────────────────────────────────
        private void NavigateTo(NavigationPage page, bool reload = false)
        {
            mainNavigationFrame.SelectedPage = page;
            if (reload) RefreshData();
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) => NavigateTo(pageDashboard);
        private void btnAccountManagement_Click_1(object sender, EventArgs e) => NavigateTo(pageAccountManagement, true);
        private void btnClearanceSystem_Click(object sender, EventArgs e) => NavigateTo(pageClearanceSystem, true);

        // ── Data ──────────────────────────────────────────────────────
        private void RefreshData()
        {
            // Evaluates the active clearance window context
            var activePeriod = _sysRepo.GetActivePeriodSettings();
            if (activePeriod != null && activePeriod.Semester.Trim().Equals("1st Semester", StringComparison.OrdinalIgnoreCase))
            {
                // Promotes any newly registered students who haven't been processed yet for this year
                _sysRepo.ForceExecutePromotion(activePeriod.AcademicYear);
            }

            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");
            LoadCurrentSystemSettings();
            LoadDashboardStats();
        }

        // ── Identity ──────────────────────────────────────────────────
        private void SetupIdentity()
        {
            if (Session.CurrentUser == null) return;
            lblFullName.Text = Session.CurrentUser.FullName;
            lblRole.Text = Session.CurrentUser.Role;
            this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
        }

        private void LoadCurrentSystemSettings()
        {
            clearancePeriodList.DataSource = _sysRepo.GetAllPeriods()
                .Select(p => new
                {
                    p.Semester,
                    p.AcademicYear,
                    Status = p.IsActive == 1 ? "ACTIVE" : "Closed"
                }).ToList();
        }

        private void LoadDashboardStats()
        {
            lblOfficeCleared.Text = _userRepo.GetUserCount("Student").ToString();
            lblStatOfficeCount.Text = _userRepo.GetUserCount("Staff").ToString();
            lblStatNewRegCount.Text = _userRepo.GetNewRegistrationsThisWeek().ToString();
            lblStatTotalCount.Text = _userRepo.GetUserCount("All").ToString();
            gcRegisteredThisWeek.DataSource = _userRepo.GetUsersRegisteredThisWeek();

            var period = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
            bool isOpen = period != null;

            lblClearanceStatus.Text = isOpen ? "Clearance System is OPEN" : "Clearance System is CLOSED";
            lblActivePeriodInfo.Text = isOpen
                ? $"Current period: {period.Semester} — {period.AcademicYear}"
                : "No active clearance period. Set one in Clearance System settings.";
        }

        // ── Grid Helpers ──────────────────────────────────────────────
        private GridView ActiveView =>
            tabPane1.SelectedPage?.Caption == "Students" ? gvStudents : gvOffice;

        private void EvaluateHitInfo(GridView view, System.Drawing.Point pt)
        {
            if (!view.CalcHitInfo(pt).InRow)
                ResetViews(gvStudents, gvOffice);
        }

        private void ResetViews(params GridView[] views)
        {
            foreach (var v in views)
            {
                v.ClearSelection();
                v.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
            }
        }

        private bool TryGetFocusedData<T>(GridView view, out T entity) where T : class
        {
            entity = view.FocusedRowHandle >= 0 ? view.GetFocusedRow() as T : null;
            return entity != null;
        }

        // ── Account CRUD ──────────────────────────────────────────────
        private void OpenUserLifecycleForm(FormMode mode, User entity = null)
        {
            using (var frm = new UserInfoForm(mode, entity) { StartPosition = FormStartPosition.CenterParent })
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e) =>
            OpenUserLifecycleForm(FormMode.Register);

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            if (TryGetFocusedData(ActiveView, out User user))
                OpenUserLifecycleForm(FormMode.Edit, user);
            else
                Notify("Please select an account row from the active view.", "Selection Required", MessageBoxIcon.Warning);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var view = ActiveView;

            var selectedUsers = view.GetSelectedRows()
                .Select(h => view.GetRow(h) as User)
                .Where(u => u != null)
                .ToList();

            if (!selectedUsers.Any())
            {
                Notify("Please select at least one account to delete.", "No Selection", MessageBoxIcon.Warning);
                return;
            }

            string names = string.Join("\n", selectedUsers.Select(u => $"• {u.FullName} ({u.UserID})"));

            if (Confirm($"Permanently delete {selectedUsers.Count} account(s)?\n\n{names}", "Confirm Deletion") != DialogResult.Yes)
                return;

            int successCount = 0;
            var failures = new System.Collections.Generic.List<string>();

            foreach (var user in selectedUsers)
            {
                try
                {
                    if (ProcessUserDeletion(user))
                        successCount++;
                }
                catch (Exception ex)
                {
                    failures.Add($"{user.FullName}: {ex.Message}");
                }
            }

            if (successCount > 0 && !failures.Any())
                Notify($"{successCount} account(s) deleted successfully.", "Deleted", MessageBoxIcon.Information);
            else if (successCount > 0 && failures.Any())
                Notify($"{successCount} account(s) deleted.\n\nFailed:\n{string.Join("\n", failures)}", "Partial Success", MessageBoxIcon.Warning);
            else
                Notify($"No accounts were deleted.\n\nErrors:\n{string.Join("\n", failures)}", "Deletion Failed", MessageBoxIcon.Error);

            RefreshData();
        }

        private bool ProcessUserDeletion(User user)
        {
            try
            {
                return DeleteUser(user.UserID, forceClearRecords: false);
            }
            catch (Exception ex) when (ex.Message.Contains("FOREIGN KEY") || ex.Message.Contains("19"))
            {
                if (Confirm($"'{user.FullName}' has active records. Force deletion will purge all related records. Proceed?",
                    "Dependencies Encountered", MessageBoxIcon.Warning) == DialogResult.Yes)
                    return DeleteUser(user.UserID, forceClearRecords: true);

                return false;
            }
        }

        private bool DeleteUser(string userId, bool forceClearRecords)
        {
            if (forceClearRecords) _clearanceRepo.DeleteRequestsByStudent(userId);
            return _userRepo.DeleteUser(userId);
        }

        // ── Document View ─────────────────────────────────────────────
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (TryGetFocusedData(gvStudents, out User student) && !string.IsNullOrEmpty(student.UploadPath))
                DocumentService.ViewDocument(student.UploadPath);
            else
                Notify("Target document path is null or corrupt.", "File Error", MessageBoxIcon.Error);
        }

        // ── Clearance Period ──────────────────────────────────────────
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            string targetSem = comboSemester.Text.Trim();
            string targetYear = comboAcademicYear.Text.Trim();

            if (string.IsNullOrEmpty(targetSem) || string.IsNullOrEmpty(targetYear))
            {
                Notify("Please select both a Semester and a School Year.", "Required Fields", MessageBoxIcon.Warning);
                return;
            }

            string confirmMsg = $"Are you sure you want to open clearance period settings for {targetSem} ({targetYear})?";

            if (targetSem.Equals("1st Semester", StringComparison.OrdinalIgnoreCase))
            {
                confirmMsg += "\n\n⚠️ SYSTEM PROMOTION NOTICE:\nBecause this is the 1st Semester, continuing student classifications (1st, 2nd, 3rd Year) will automatically advance.";
            }

            if (Confirm(confirmMsg, "Confirm Clearance Configuration Opening", MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!_sysRepo.CreateNewPeriod(targetSem, targetYear))
            {
                Notify($"{targetSem} — {targetYear} already exists.\n\nDelete it first before creating a new period.",
                    "Duplicate Period", MessageBoxIcon.Warning);
                return;
            }

            RefreshData();
        }

        private void btnClosePeriod_Click(object sender, EventArgs e)
        {
            var period = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
            if (period == null)
            {
                Notify("There is no active clearance period to close.", "Nothing to Close", MessageBoxIcon.Information);
                return;
            }

            string msg = $"Close the current period?\n\n{period.Semester} — {period.AcademicYear}\n\n" +
                         "Students will no longer be able to submit clearance requests.";

            if (Confirm(msg, "Confirm Close Period", MessageBoxIcon.Warning) != DialogResult.Yes) return;

            if (_sysRepo.CloseActivePeriod())
            {
                Notify("Clearance period has been closed successfully.", "Period Closed", MessageBoxIcon.Information);
                RefreshData();
            }
            else
            {
                Notify("Failed to close the period. Please try again.", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnDeleteSettings_Click(object sender, EventArgs e)
        {
            int handle = tileView1.FocusedRowHandle;
            if (handle < 0)
            {
                Notify("Please select a period from the list first.", "No Selection", MessageBoxIcon.Warning);
                return;
            }

            string semester = tileView1.GetRowCellValue(handle, "Semester")?.ToString();
            string year = tileView1.GetRowCellValue(handle, "AcademicYear")?.ToString();
            string status = tileView1.GetRowCellValue(handle, "Status")?.ToString();

            if (status == "ACTIVE")
            {
                Notify("Cannot delete an active clearance period.\n\nClose it first before deleting.",
                    "Period Still Active", MessageBoxIcon.Warning);
                return;
            }

            if (Confirm($"Delete period: {semester} — {year}?\n\nThis cannot be undone.",
                "Confirm Delete", MessageBoxIcon.Warning) != DialogResult.Yes) return;

            if (_sysRepo.DeletePeriod(semester, year))
            {
                Notify("Period deleted successfully.", "Deleted", MessageBoxIcon.Information);
                RefreshData();
            }
            else
            {
                Notify("Failed to delete the period. Please try again.", "Error", MessageBoxIcon.Error);
            }
        }

        // ── Logout ────────────────────────────────────────────────────
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (Confirm("Are you sure you want to log out?", "Logout") != DialogResult.Yes) return;
            new Login().Show();
            Hide();
        }

        // ── Helpers ───────────────────────────────────────────────────
        private void Notify(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Asterisk) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.OK, icon);

        private DialogResult Confirm(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Question) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.YesNo, icon);
    }
}