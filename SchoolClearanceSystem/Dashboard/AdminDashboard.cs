using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using SchoolClearanceSystem.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SchoolClearanceSystem.Dashboard
{
    
    public partial class AdminDashboard : XtraForm
    {
        // ENCAPSULATION: Readonly repository layers for handling database interactions.
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public AdminDashboard()
        {
            InitializeComponent();

            gcStudents.MouseDown += (s, e) => EvaluateHitInfo(gvStudents, e.Location);
            gcOffice.MouseDown += (s, e) => EvaluateHitInfo(gvOffice, e.Location);
            txtSearch.TextChanged += (s, e) => ApplyUnifiedFilter();

            RefreshData();
        }

        /// <summary>
        /// Overrides the WinForms Form OnLoad cycle. 
        /// Validates that the application isn't in Visual Studio Designer mode and verifies an active session before personalization.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode && Session.CurrentUser != null) SetupIdentity();
        }

        /// <summary>
        /// Visual Customization: Displays the logged-in administrator's full name, role, 
        /// and dynamic titles across dashboard labels and the main Form window header.
        /// </summary>
        private void SetupIdentity()
        {
            lblFullName.Text = Session.CurrentUser.FullName;
            lblRole.Text = Session.CurrentUser.Role;
            this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
        }

        /// <summary>
        /// Evaluates the input string from the central search bar and uses DevExpress's built-in 
        /// Find Filter mechanism to instantly match records inside the currently visible GridView.
        /// </summary>
        private void ApplyUnifiedFilter()
        {
            try { ActiveView?.ApplyFindFilter(txtSearch.Text.Trim()); }
            catch (Exception ex) { UIHelper.Notify($"Could not apply filter: {ex.Message}", "Filter Error", MessageBoxIcon.Error); }
        }

        /// <summary>
        /// Explicit Click Handler for the Search Button: Backup mechanism to trigger 
        /// the unified text grid filter evaluation manually.
        /// </summary>
        private void btnSearch_Click_1(object sender, EventArgs e) => ApplyUnifiedFilter();

        /// <summary>
        /// Central Navigational Wrapper: Swaps the visible view of the central DevExpress NavigationFrame.
        /// Optionally forces a structural data reload from repositories to update analytics grids.
        /// </summary>
        private void NavigateTo(NavigationPage page, bool reload = false)
        {
            mainNavigationFrame.SelectedPage = page;
            if (reload) RefreshData();
        }

        /// <summary> Navigation Button Click: Routes the layout interface to the main KPI Analytics Dashboard. </summary>
        private void btnDashboard_Click_1(object sender, EventArgs e) => NavigateTo(pageDashboard);

        /// <summary> Navigation Button Click: Routes to the Account Management workspace and forces fresh data reload. </summary>
        private void btnAccountManagement_Click_1(object sender, EventArgs e) => NavigateTo(pageAccountManagement, true);

        /// <summary> Navigation Button Click: Routes to the System Clearance Cycle configuration tools and reloads timelines. </summary>
        private void btnClearanceSystem_Click(object sender, EventArgs e) => NavigateTo(pageClearanceSystem, true);

        /// <summary>
        /// Higher-Order Wrapper Method: Executes structural database mutations (like closing or deleting clearance timelines) 
        /// through a functional delegate. Handles global logging alerts and triggers data updates upon success.
        /// </summary>
        private void ExecutePeriodAction(string successMsg, Func<bool> repoAction)
        {
            if (repoAction())
            {
                UIHelper.Notify(successMsg, "Operation Successful", MessageBoxIcon.Information);
                RefreshData();
            }
            else
            {
                UIHelper.Notify("Action failed. Please try again.", "Error", MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Central Data Coordinator: Repopulates both student and staff grids via role-based queries, 
        /// and updates metadata lists alongside visual numerical analytical statistics.
        /// </summary>
        private void RefreshData()
        {
            var activePeriod = _sysRepo.GetActivePeriodSettings();

            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");

            LoadCurrentSystemSettings();
            LoadDashboardStats();
        }

        /// <summary>
        /// Maps system configuration periods directly into the DevExpress Grid / Tile control, 
        /// converting native numerical flags into highly readable status strings (ACTIVE / Closed).
        /// </summary>
        private void LoadCurrentSystemSettings()
        {
            clearancePeriodList.DataSource = _sysRepo.GetAllPeriods()
                .Select(p => new { p.Semester, p.AcademicYear, Status = p.IsActive == 1 ? "ACTIVE" : "Closed" })
                .ToList();
        }

        /// <summary>
        /// Metrics Processor: Queries database volumes to update KPI dashboard badges, pulls recent users 
        /// registered within the current calendar week, and tracks system state messages based on open calendars.
        /// </summary>
        private void LoadDashboardStats()
        {
            lblOfficeCleared.Text = _userRepo.GetUserCount("Student").ToString();
            lblStatOfficeCount.Text = _userRepo.GetUserCount("Staff").ToString();
            lblStatNewRegCount.Text = _userRepo.GetNewRegistrationsThisWeek().ToString();
            lblStatTotalCount.Text = _userRepo.GetUserCount("All").ToString();
            gcRegisteredThisWeek.DataSource = _userRepo.GetRecentUsers();

            var period = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
            bool isOpen = period != null;

            lblClearanceStatus.Text = isOpen ? "Clearance System is OPEN" : "Clearance System is CLOSED";
            lblActivePeriodInfo.Text = isOpen
                ? $"Current period: {period.Semester} — {period.AcademicYear}"
                : "No active clearance period. Set one in Clearance System settings.";
        }

        /// <summary>
        /// Evaluator Property: Inspects the UI TabPane container selection state to programmatically determine 
        /// which DevExpress GridView (Students vs Office/Staff) is currently focused for commands.
        /// </summary>
        private GridView ActiveView => (tabPane1.SelectedPage?.Caption == "Students") ? gvStudents : gvOffice;

        /// <summary>
        /// DevExpress HitInfo Evaluator: Analyzes screen coordinates during click events. If an administrator clicks 
        /// on dead space inside a grid container rather than an actual populated data row, selections are cleared out.
        /// </summary>
        private void EvaluateHitInfo(GridView view, System.Drawing.Point pt)
        {
            if (!view.CalcHitInfo(pt).InRow)
            {
                foreach (var v in new[] { gvStudents, gvOffice })
                {
                    v.ClearSelection();
                    v.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
                }
            }
        }

        /// <summary>
        /// Type-Safe Selection Extractor: Extracts the underlying object bind mapped to the focused 
        /// row of the active DevExpress grid container and returns validation tracking safely.
        /// </summary>
        private bool TryGetFocusedData<T>(GridView view, out T entity) where T : class
        {
            entity = view.FocusedRowHandle >= 0 ? view.GetFocusedRow() as T : null;
            return entity != null;
        }

        /// <summary>
        /// Lifecycle Dialog Instantiator: Builds and launches the secondary User Information modal window 
        /// in either Registration or Modification context, passing reference records and reloading data upon confirmation.
        /// </summary>
        private void OpenUserLifecycleForm(FormMode mode, User entity = null)
        {
            using (var frm = new UserInfoForm(mode, entity) { StartPosition = FormStartPosition.CenterParent })
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
        }

        /// <summary> Click Handler: Spawns the identity creation form window empty in structural "Register" mode. </summary>
        private void btnRegisterAccount_Click(object sender, EventArgs e) => OpenUserLifecycleForm(FormMode.Register);

        /// <summary>
        /// Click Handler: Attempts to resolve the highlighted entity on the active grid. If found, 
        /// passes record references to the user layout form tailored in structural "Edit" mode.
        /// </summary>
        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            if (TryGetFocusedData(ActiveView, out User user)) OpenUserLifecycleForm(FormMode.Edit, user);
            else UIHelper.Notify("Please select an account row from the active view.", "Selection Required", MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Click Handler (Bulk Deletion): Pulls multiple highlighted rows from the grid view, runs safety confirmation loops, 
        /// loops individual deletions through database wrappers, and tracks any relational failure outputs.
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var view = ActiveView;
            var selectedUsers = view.GetSelectedRows().Select(h => view.GetRow(h) as User).Where(u => u != null).ToList();

            if (!selectedUsers.Any())
            {
                UIHelper.Notify("Please select at least one account to delete.", "No Selection", MessageBoxIcon.Warning);
                return;
            }

            string names = string.Join("\n", selectedUsers.Select(u => $"• {u.FullName} ({u.UserID})"));
            if (UIHelper.Confirm($"Permanently delete {selectedUsers.Count} account(s)?\n\n{names}", "Confirm Deletion") != DialogResult.Yes) return;

            int successCount = 0;
            var failures = new List<string>();

            foreach (var user in selectedUsers)
            {
                try { if (ProcessUserDeletion(user)) successCount++; }
                catch (Exception ex) { failures.Add($"{user.FullName}: {ex.Message}"); }
            }

            if (successCount > 0 && !failures.Any())
                UIHelper.Notify($"{successCount} account(s) deleted successfully.", "Deleted", MessageBoxIcon.Information);
            else
                UIHelper.Notify(successCount > 0 ? $"{successCount} account(s) deleted.\n\nFailed:\n{string.Join("\n", failures)}" : $"No accounts were deleted.\n\nErrors:\n{string.Join("\n", failures)}", successCount > 0 ? "Partial Success" : "Deletion Failed", successCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Error);

            RefreshData();
        }

        /// <summary>
        /// Structural Dependency Resolution Processor: Attempts immediate removal of user record IDs. If SQL Foreign Key constraints 
        /// trip due to attached clearable requests, it prompts confirmation for an administrative database cascade purge.
        /// </summary>
        private bool ProcessUserDeletion(User user)
        {
            try { return _userRepo.DeleteUser(user.UserID); }
            catch (Exception ex) when (ex.Message.Contains("FOREIGN KEY") || ex.Message.Contains("19"))
            {
                if (UIHelper.Confirm($"'{user.FullName}' has active records. Force deletion will purge all related records. Proceed?", "Dependencies Encountered", MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _clearanceRepo.DeleteRequestsByStudent(user.UserID);
                    return _userRepo.DeleteUser(user.UserID);
                }
                return false;
            }
        }

        /// <summary>
        /// Click Handler: Evaluates inputs from semester/year dropdown controls. If parameters pass layout validation check rules, 
        /// creates a fresh clearance timeline record inside the database unless a replica definition already blocks it.
        /// </summary>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            string targetSem = comboSemester.Text.Trim();
            string targetYear = comboAcademicYear.Text.Trim();

            if (string.IsNullOrEmpty(targetSem) || string.IsNullOrEmpty(targetYear))
            {
                UIHelper.Notify("Please select both a Semester and a School Year.", "Required Fields", MessageBoxIcon.Warning);
                return;
            }

            string confirmMsg = $"Are you sure you want to open the clearance period for {targetSem} ({targetYear})?";

            if (UIHelper.Confirm(confirmMsg, "Confirm System Opening", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!_sysRepo.CreateNewPeriod(targetSem, targetYear))
                    UIHelper.Notify($"{targetSem} — {targetYear} already exists.\n\nDelete it first before creating a new period.", "Duplicate Period", MessageBoxIcon.Warning);
                else
                    RefreshData();
            }
        }

        /// <summary>
        /// Click Handler: Looks up active records in the clearance timeline database view. 
        /// If found, shuts down global student submissions by turning off activation status attributes on the server.
        /// </summary>
        private void btnClosePeriod_Click(object sender, EventArgs e)
        {
            var period = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
            if (period == null)
            {
                UIHelper.Notify("There is no active clearance period to close.", "Nothing to Close", MessageBoxIcon.Information);
                return;
            }

            if (UIHelper.Confirm($"Close the current period?\n\n{period.Semester} — {period.AcademicYear}\n\nStudents will no longer be able to submit clearance requests.", "Confirm Close Period", MessageBoxIcon.Warning) == DialogResult.Yes)
                ExecutePeriodAction("Clearance period has been closed successfully.", () => _sysRepo.CloseActivePeriod());
        }

        /// <summary>
        /// Click Handler: Identifies the targeted history timeline from a focused item inside DevExpress TileView layout control, 
        /// ensures running active semesters are locked from deletion safety violations, and purges past closed schedules.
        /// </summary>
        private void btnDeleteSettings_Click(object sender, EventArgs e)
        {
            int handle = tileView1.FocusedRowHandle;
            if (handle < 0)
            {
                UIHelper.Notify("Please select a period from the list first.", "No Selection", MessageBoxIcon.Warning);
                return;
            }

            string sem = tileView1.GetRowCellValue(handle, "Semester")?.ToString();
            string yr = tileView1.GetRowCellValue(handle, "AcademicYear")?.ToString();

            if (tileView1.GetRowCellValue(handle, "Status")?.ToString() == "ACTIVE")
            {
                UIHelper.Notify("Cannot delete an active clearance period.\n\nClose it first before deleting.", "Period Still Active", MessageBoxIcon.Warning);
                return;
            }

            if (UIHelper.Confirm($"Delete period: {sem} — {yr}?\n\nThis cannot be undone.", "Confirm Delete", MessageBoxIcon.Warning) == DialogResult.Yes)
                ExecutePeriodAction("Period deleted successfully.", () => _sysRepo.DeletePeriod(sem, yr));
        }

        /// <summary>
        /// Click Handler: Requests security sign-out confirmation, returns the application lifecycle state 
        /// back to an open instance of the Login interface, and hides the current active administrator workspace container.
        /// </summary>
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UIHelper.Confirm("Are you sure you want to log out?", "Logout") != DialogResult.Yes) return;
            new Login().Show();
            Hide();
        }
    }
}