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
        // FIX: C# 7.3 requires explicit type names for initialization (No target-typed new())
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();

            // Declarative event registrations
            gcStudents.MouseDown += (s, e) => EvaluateHitInfo(gvStudents, e.Location);
            gcOffice.MouseDown += (s, e) => EvaluateHitInfo(gvOffice, e.Location);
            tabPane1.SelectedPageChanged += (s, e) => ResetViews(gvStudents, gvOffice);
            listBoxAdminHistory.ContextButtonClick += OnHistoryContextClicked;
        }

        // ── Navigation & Presentation ───────────────────────────────────────────────
        private void NavigateTo(NavigationPage page, bool reload = false)
        {
            mainNavigationFrame.SelectedPage = page;
            if (reload) RefreshData();
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) => NavigateTo(pageDashboard);
        private void btnAccountManagement_Click_1(object sender, EventArgs e) => NavigateTo(pageAccountManagement, true);
        private void btnClearanceSystem_Click(object sender, EventArgs e) => NavigateTo(pageClearanceSystem, true);

        private void RefreshData()
        {
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");
            LoadCurrentSystemSettings();
        }

        // ── Grid Controller Actions ──────────────────────────────────────────────────
        private GridView ActiveView => tabPane1.SelectedPage.Caption == "Students" ? gvStudents : gvOffice;

        private void EvaluateHitInfo(GridView view, System.Drawing.Point pt)
        {
            if (!view.CalcHitInfo(pt).InRow) ResetViews(gvStudents, gvOffice);
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

        // ── Account Lifecycle (CRUD Management) ──────────────────────────────────────
        private void OpenUserLifecycleForm(FormMode mode, User entity = null)
        {
            // FIX: Reverted 'using declaration' back to a standard C# 7.3 using block statement
            using (UserInfoForm frm = new UserInfoForm(mode, entity))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
            }
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e) => OpenUserLifecycleForm(FormMode.Register);

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            if (TryGetFocusedData(ActiveView, out User targetedUser))
                OpenUserLifecycleForm(FormMode.Edit, targetedUser);
            else
                Notify("Please select an account row from the active view.", "Selection Required", MessageBoxIcon.Warning);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!TryGetFocusedData(ActiveView, out User user)) return;

            string msg = "Permanently remove account: " + user.FullName + " (" + user.UserID + ")?";
            if (Confirm(msg, "Confirm Deletion") != DialogResult.Yes) return;

            ProcessUserPurgePipeline(user.UserID);
        }

        private void ProcessUserPurgePipeline(string userId)
        {
            try
            {
                ExecutePurge(userId, false);
            }
            catch (Exception ex) when (ex.Message.Contains("FOREIGN KEY") || ex.Message.Contains("19"))
            {
                string msg = "This student has active records. Force deletion will purge all tracking files. Proceed?";
                if (Confirm(msg, "Dependencies Encountered", MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ExecutePurge(userId, true);
                }
            }
            catch (Exception ex)
            {
                Notify("Execution Error: " + ex.Message, "Pipeline Failure", MessageBoxIcon.Error);
            }
        }

        private void ExecutePurge(string uid, bool forcePurgeDependency)
        {
            if (forcePurgeDependency) _clearanceRepo.DeleteRequestsByStudent(uid);

            if (_userRepo.DeleteUser(uid))
            {
                Notify("Account operational sequence terminated successfully.", "Purged", MessageBoxIcon.Information);
                RefreshData();
            }
        }

        // ── Document Access & History Feed ───────────────────────────────────────────
        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (TryGetFocusedData(gvStudents, out User student) && !string.IsNullOrEmpty(student.UploadPath))
                DocumentService.ViewDocument(student.UploadPath);
            else
                Notify("Target document path null or corrupt.", "File Error", MessageBoxIcon.Error);
        }

        private void LoadCurrentSystemSettings()
        {
            listBoxAdminHistory.DataSource = _sysRepo.GetAllPeriods().Select(p => new ClearanceHistoryViewModel
            {
                PeriodName = "ℹ️  " + p.AcademicYear + " " + p.Semester,
                StatusText = p.IsActive == 1 ? "Clearance Processing Active" : "Clearance Done"
            }).ToList();
        }

        private void OnHistoryContextClicked(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            if ((e.Item.Name == "View" || e.Item.Name == "View") && e.DataItem is ClearanceHistoryViewModel historicalContext)
            {
                Notify("Context loaded: " + historicalContext.PeriodName, "Pipeline Engine Active", MessageBoxIcon.Information);
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboSemester.Text) || string.IsNullOrEmpty(comboSchoolYear.Text)) return;

            if (_sysRepo.CreateNewPeriod(comboSemester.Text, comboSchoolYear.Text)) RefreshData();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (Confirm("Are you sure you want to log out?", "Logout") != DialogResult.Yes) return;
            new Login().Show();
            Hide();
        }

        // ── Encapsulated Message Notification Wrappers ──────────────────────────────
        private void Notify(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Asterisk) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.OK, icon);

        private DialogResult Confirm(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Question) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.YesNo, icon);
    }
}