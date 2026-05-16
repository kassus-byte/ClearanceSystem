using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars.Navigation;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository; // CONNECTS TO: Data Access Layer
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    /// <summary>
    /// OOP CONCEPT: SEPARATION OF CONCERNS (3-Tier Architecture)
    /// This dashboard is strictly a Presentation Layer form. It contains zero SQL statements 
    /// and doesn't know what kind of database you use. It relies entirely on middlemen 
    /// classes (Repositories) to fetch and push data.
    /// </summary>
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// OOP CONCEPT: COMPOSITION / HAS-A RELATIONSHIP
        /// The AdminDashboard "has a" UserRepository and a SystemRepository.
        /// By keeping these instances as private fields, we encapsulate data access behaviors 
        /// safely inside this specific form context.
        /// </summary>
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData(); // Automatically populates data grids upon initialization

            // CONNECTS TO DATABASE PATH VIA: SystemRepository -> BaseRepository -> DatabaseManager
            tsStatus.IsOn = _sysRepo.IsClearanceActive();
            SetupGridBehaviors();
        }

        // Inline Expression: Updates UI state based on menu click event
        private void btnDashboard_Click_1(object sender, EventArgs e) => mainNavigationFrame.SelectedPage = pageDashboard;

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
            RefreshData(); // Synchronizes the data grids with latest database records
        }

        /// <summary>
        /// HOW IT CONNECTS TO DATABASE MANAGER:
        /// 1. This form asks '_userRepo' for specific role groupings.
        /// 2. 'UserRepository' constructs an SQL string and requests an IDbConnection pipeline.
        /// 3. 'DatabaseManager' wakes up 'SqliteConnection', matches the local path string, 
        ///    and lets Dapper map database rows into a clean, bindable 'List<User>'.
        /// 4. This form safely binds that List directly to the DevExpress GridControl DataSource.
        /// </summary>
        private void RefreshData()
        {
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student", true);
            gcOffice.DataSource = _userRepo.GetUsersByRole("", false);
        }

        private void SetupGridBehaviors()
        {
            // Lambda Expression tracking mouse coordinates to clear selections when whitespace is clicked
            gcStudents.MouseDown += (s, e) => {
                var hitInfo = gvStudents.CalcHitInfo(e.Location);
                if (!hitInfo.InRow) ClearAllSelections();
            };

            gcOffice.MouseDown += (s, e) => {
                var hitInfo = gvOffice.CalcHitInfo(e.Location);
                if (!hitInfo.InRow) ClearAllSelections();
            };
        }

        private void ClearAllSelections()
        {
            gvStudents.ClearSelection();
            gvStudents.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;

            gvOffice.ClearSelection();
            gvOffice.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void tabPane1_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            ClearAllSelections();
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            // OOP CONCEPT: ENCAPSULATION VIA OBJECT INITIALIZER & STATE PASSING
            // We launch the UserInfoForm passing FormMode.Register state and a null data entity.
            using (UserInfoForm frm = new UserInfoForm(FormMode.Register, null))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                // If modal dialog finishes successfully with DialogResult.OK, refresh data grids automatically
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            // Evaluation logic checking current open UI Tab to isolate row interactions
            var activeView = (tabPane1.SelectedPage.Caption == "Students") ? gvStudents : gvOffice;

            // OOP CONCEPT: POLYMORPHISM / PATTERN MATCHING
            // 'activeView.GetFocusedRow()' returns a generic system object. 
            // The C# keyword 'is User selectedUser' safely type-casts it into a concrete 'User' model instance.
            if (activeView.FocusedRowHandle >= 0 && activeView.GetFocusedRow() is User selectedUser)
            {
                // Launches identical form view but updates execution state parameter to 'FormMode.Edit'
                using (UserInfoForm frm = new UserInfoForm(FormMode.Edit, selectedUser))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
                }
            }
            else
            {
                XtraMessageBox.Show("Please select an account from the current list to edit.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsStatus_Toggled(object sender, EventArgs e)
        {
            // CONNECTS TO DATABASE MANAGER: Fires an execution command down to change system boolean status flags
            _sysRepo.ToggleClearanceSeason(tsStatus.IsOn);
            string status = tsStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance season is now {status}.", "System Update",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            // Extracts current dynamic table row and casts it into an individual User instance profile
            if (gvStudents.GetFocusedRow() is User selectedUser)
            {
                try
                {
                    // OOP CONCEPT: ABSTRACTION
                    // DocumentService hides the OS complexity of running shell executions.
                    // The form does not care how Windows allocates thread memory to draw an image viewer;
                    // it simply sends a file path string to DocumentService.
                    if (!string.IsNullOrEmpty(selectedUser.UploadPath))
                        DocumentService.ViewDocument(selectedUser.UploadPath);
                    else
                        XtraMessageBox.Show("No document found.", "Error");
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // OOP State Transition: Dispose/hide current layout framework context and switch roots
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}