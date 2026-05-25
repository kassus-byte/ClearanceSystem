using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : XtraForm
    {
        // ── Repositories (Encapsulation — all DB access hidden behind these) ─────────
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();

            // Wire grid mouse events (deselects row when clicking empty space)
            gcStudents.MouseDown += (s, e) => EvaluateHitInfo(gvStudents, e.Location);
            gcOffice.MouseDown += (s, e) => EvaluateHitInfo(gvOffice, e.Location);

            // Reset both grid selections when switching between Students/Office tabs
            tabPane1.SelectedPageChanged += (s, e) => ResetViews(gvStudents, gvOffice);

            listBoxAdminHistory.ContextButtonClick += OnHistoryContextClicked;
        }

        // ── Navigation ────────────────────────────────────────────────────────────────
        // Abstraction: one method handles all page switching + optional reload
        private void NavigateTo(NavigationPage page, bool reload = false)
        {
            mainNavigationFrame.SelectedPage = page;
            if (reload) RefreshData();
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) => NavigateTo(pageDashboard);
        private void btnAccountManagement_Click_1(object sender, EventArgs e) => NavigateTo(pageAccountManagement, true);
        private void btnClearanceSystem_Click(object sender, EventArgs e) => NavigateTo(pageClearanceSystem, true);

        // ── Core Reload ───────────────────────────────────────────────────────────────
        // Single method that refreshes every section of the dashboard at once.
        // Called on load, and after every create/edit/delete/period change.
        private void RefreshData()
        {
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");
            LoadCurrentSystemSettings();
            LoadDashboardStats();
        }

        // ── Grid Helpers ──────────────────────────────────────────────────────────────
        // Computed property (Abstraction): returns the currently active grid view
        // based on which tab the admin is on — no need to check this manually elsewhere
        private GridView ActiveView => tabPane1.SelectedPage.Caption == "Students" ? gvStudents : gvOffice;

        private void EvaluateHitInfo(GridView view, System.Drawing.Point pt)
        {
            if (!view.CalcHitInfo(pt).InRow) ResetViews(gvStudents, gvOffice);
        }

        // Polymorphism: accepts any number of GridView arguments via params
        private void ResetViews(params GridView[] views)
        {
            foreach (var v in views)
            {
                v.ClearSelection();
                v.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
            }
        }

        // Generics (Abstraction): one method safely reads any focused grid row as any type
        private bool TryGetFocusedData<T>(GridView view, out T entity) where T : class
        {
            entity = view.FocusedRowHandle >= 0 ? view.GetFocusedRow() as T : null;
            return entity != null;
        }

        // ── Account CRUD ──────────────────────────────────────────────────────────────
        // Abstraction: both Register and Edit flow through one method,
        // FormMode enum decides which behavior UserInfoForm uses
        private void OpenUserLifecycleForm(FormMode mode, User entity = null)
        {
            using (var frm = new UserInfoForm(mode, entity) { StartPosition = FormStartPosition.CenterParent })
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e) => OpenUserLifecycleForm(FormMode.Register);

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            if (TryGetFocusedData(ActiveView, out User user))
                OpenUserLifecycleForm(FormMode.Edit, user);
            else
                Notify("Please select an account row from the active view.", "Selection Required", MessageBoxIcon.Warning);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!TryGetFocusedData(ActiveView, out User user)) return;
            if (Confirm($"Permanently remove account: {user.FullName} ({user.UserID})?", "Confirm Deletion") == DialogResult.Yes)
                ProcessUserPurgePipeline(user.UserID);
        }

        // Handles FK constraint errors gracefully — offers force delete if needed
        private void ProcessUserPurgePipeline(string userId)
        {
            try
            {
                ExecutePurge(userId, false);
            }
            catch (Exception ex) when (ex.Message.Contains("FOREIGN KEY") || ex.Message.Contains("19"))
            {
                if (Confirm("This student has active records. Force deletion will purge all tracking files. Proceed?",
                    "Dependencies Encountered", MessageBoxIcon.Warning) == DialogResult.Yes)
                    ExecutePurge(userId, true);
            }
            catch (Exception ex)
            {
                Notify($"Execution Error: {ex.Message}", "Pipeline Failure", MessageBoxIcon.Error);
            }
        }

        private void ExecutePurge(string uid, bool forcePurgeDependency)
        {
            if (forcePurgeDependency) _clearanceRepo.DeleteRequestsByStudent(uid);
            if (_userRepo.DeleteUser(uid))
            {
                Notify("Account successfully deleted.", "Deleted", MessageBoxIcon.Information);
                RefreshData();
            }
        }

        // ── Dashboard Stats ───────────────────────────────────────────────────────────
        // Updates all 4 stat cards, the registered accounts grid,
        // and the clearance period banner in one pass.
        private void LoadDashboardStats()
        {
            lblOfficeCleared.Text = _userRepo.GetUserCount("Student").ToString();
            labelControl15.Text = _userRepo.GetUserCount("Staff").ToString();
            labelControl16.Text = _userRepo.GetNewRegistrationsThisWeek().ToString();
            labelControl17.Text = _userRepo.GetUserCount("All").ToString();

            gridControl1.DataSource = _userRepo.GetUsersRegisteredThisWeek();

            // Banner: OPEN if an active period exists, CLOSED if not
            var period = _sysRepo.GetAllPeriods().FirstOrDefault(p => p.IsActive == 1);
            bool isOpen = period != null;

            lblClearanceStatus.Text = isOpen ? "Clearance System is OPEN" : "Clearance System is CLOSED";
            labelControl19.Text = isOpen
                ? $"Current period: {period.Semester} — {period.AcademicYear}"
                : "No active clearance period. Set one in Clearance System settings.";
        }

        // ── Document Viewer ───────────────────────────────────────────────────────────
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (TryGetFocusedData(gvStudents, out User student) && !string.IsNullOrEmpty(student.UploadPath))
                DocumentService.ViewDocument(student.UploadPath);
            else
                Notify("Target document path null or corrupt.", "File Error", MessageBoxIcon.Error);
        }

        // ── Clearance Period History ───────────────────────────────────────────────────
        private void LoadCurrentSystemSettings()
        {
            listBoxAdminHistory.DataSource = _sysRepo.GetAllPeriods()
                .Select(p => new ClearanceHistoryViewModel
                {
                    PeriodName = $"ℹ️  {p.AcademicYear} {p.Semester}",
                    StatusText = p.IsActive == 1 ? "Clearance Processing Active" : "Clearance Done"
                }).ToList();
        }

        private void OnHistoryContextClicked(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            if (e.Item.Name == "View" && e.DataItem is ClearanceHistoryViewModel ctx)
                Notify($"Context loaded: {ctx.PeriodName}", "Pipeline Engine Active", MessageBoxIcon.Information);
        }

        // ── Clearance Period Management ───────────────────────────────────────────────
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboSemester.Text) && !string.IsNullOrEmpty(comboSchoolYear.Text)
                && _sysRepo.CreateNewPeriod(comboSemester.Text, comboSchoolYear.Text))
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

            string message = $"Close the current period?\n\n{period.Semester} — {period.AcademicYear}\n\n" +
                              "Students will no longer be able to submit clearance requests.";

            if (Confirm(message, "Confirm Close Period", MessageBoxIcon.Warning) != DialogResult.Yes) return;

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

        // ── Logout ────────────────────────────────────────────────────────────────────
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (Confirm("Are you sure you want to log out?", "Logout") != DialogResult.Yes) return;
            new Login().Show();
            Hide();
        }

        // ── Message Wrappers (Abstraction) ────────────────────────────────────────────
        // Encapsulates XtraMessageBox calls so the rest of the class stays clean.
        // Every alert and confirmation goes through these two methods.
        private void Notify(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Asterisk) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.OK, icon);

        private DialogResult Confirm(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Question) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.YesNo, icon);
    }
}