using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;
using System.Linq;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();
        private readonly ClearanceRepository _clearanceRepo = new ClearanceRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();
            SetupGridBehaviors();
            LoadCurrentSystemSettings(); // Connects your visually designed ListBox data feed
        }

        private void btnDashboard_Click_1(object sender, EventArgs e) => mainNavigationFrame.SelectedPage = pageDashboard;

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
            RefreshData();
        }

        private void RefreshData()
        {
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");
        }

        private void SetupGridBehaviors()
        {
            gcStudents.MouseDown += (s, e) => HandleGridSelection(gvStudents, e.Location);
            gcOffice.MouseDown += (s, e) => HandleGridSelection(gvOffice, e.Location);

            // Wire up the visual designer row button click listener
            listBoxAdminHistory.ContextButtonClick += listBoxAdminHistory_ContextButtonClick;
        }

        private void HandleGridSelection(GridView view, System.Drawing.Point location)
        {
            var hitInfo = view.CalcHitInfo(location);
            if (!hitInfo.InRow) ClearAllSelections();
        }

        private void ClearAllSelections()
        {
            ResetGridView(gvStudents);
            ResetGridView(gvOffice);
        }

        private void ResetGridView(GridView view)
        {
            view.ClearSelection();
            view.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        }

        private void tabPane1_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e) => ClearAllSelections();

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            OpenUserForm(FormMode.Register, null);
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            var activeView = GetActiveGridView();

            if (TryGetSelectedUser(activeView, out User selectedUser))
            {
                OpenUserForm(FormMode.Edit, selectedUser);
            }
            else
            {
                XtraMessageBox.Show("Please select an account from the current list to edit.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var activeView = GetActiveGridView();

            if (!TryGetSelectedUser(activeView, out User selectedUser))
            {
                XtraMessageBox.Show("Please select an active row record from the list before attempting deletion.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult confirm = XtraMessageBox.Show(
                $"Are you sure you want to permanently delete the account for {selectedUser.FullName} ({selectedUser.UserID})?",
                "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            ExecuteUserDeletion(selectedUser.UserID);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (gvStudents.GetFocusedRow() is User selectedUser)
            {
                try
                {
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
            DialogResult result = XtraMessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                new Login().Show();
                this.Hide();
            }
        }

        // ── OOP Architectural Helpers ──────────────────────────────────────────────────

        private GridView GetActiveGridView()
        {
            return (tabPane1.SelectedPage.Caption == "Students") ? gvStudents : gvOffice;
        }

        private bool TryGetSelectedUser(GridView view, out User user)
        {
            user = null;
            if (view.FocusedRowHandle >= 0 && view.GetFocusedRow() is User selectedUser)
            {
                user = selectedUser;
                return true;
            }
            return false;
        }

        private void OpenUserForm(FormMode mode, User user)
        {
            using (UserInfoForm frm = new UserInfoForm(mode, user))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
            }
        }

        private void ExecuteUserDeletion(string userId)
        {
            try
            {
                if (_userRepo.DeleteUser(userId))
                {
                    XtraMessageBox.Show("Account successfully deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshData();
                }
                else
                {
                    XtraMessageBox.Show("Failed to delete the account. Please check database limits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY constraint failed") || ex.Message.Contains("19"))
                {
                    DialogResult forceDeleteConfirm = XtraMessageBox.Show(
                        "This student has ongoing clearance requests or active files inside the system.\n\n" +
                        "Do you want to proceed with a force deletion? This will automatically clear all of their ongoing requests as well.",
                        "Student Has Ongoing Requests",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (forceDeleteConfirm == DialogResult.Yes)
                    {
                        try
                        {
                            _clearanceRepo.DeleteRequestsByStudent(userId);

                            if (_userRepo.DeleteUser(userId))
                            {
                                XtraMessageBox.Show("Account and all associated clearance records have been successfully purged.",
                                                    "Force Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshData();
                            }
                        }
                        catch (Exception nestedEx)
                        {
                            XtraMessageBox.Show($"Force delete operation failed: {nestedEx.Message}", "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show($"Database tracking dependency error: {ex.Message}", "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClearanceSystem_Click(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageClearanceSystem;
            RefreshData();
        }

        // ── Chronological Clearance History UI Designer Binding Engine ─────────────────

        /// <summary>
        /// Feeds database rows into the ListBoxControl layout template designed in the UI.
        /// </summary>
        private void LoadCurrentSystemSettings()
        {
            try
            {
                // Pull raw system settings blocks from repository layer
                var historicalPeriods = _sysRepo.GetAllPeriods();

                // Map database entries into your unified UI View Model data collection format
                var viewData = historicalPeriods.Select(p => new ClearanceHistoryViewModel
                {
                    // Matches element1 data column assignment property bound via UI designer
                    PeriodName = $"ℹ️  {p.AcademicYear} {p.Semester}",

                    // Matches element3 data column assignment property bound via UI designer
                    StatusText = p.IsActive == 1 ? "Clearance Processing Active" : "Clearance Done"
                }).ToList();

                // Set DataSource. The ListBoxControl displays the UI designer layout instantly!
                listBoxAdminHistory.DataSource = viewData;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Failed to load operational clearance history records: {ex.Message}", "UI Render Error");
            }
        }

        /// <summary>
        /// Captures click choices made directly inside the dynamic list view template buttons.
        /// </summary>
        private void listBoxAdminHistory_ContextButtonClick(object sender, DevExpress.Utils.ContextItemClickEventArgs e)
        {
            // FIX: Checked against e.Item.Name ("element2") and e.Item.Caption ("View")
            if (e.Item.Name == "element2" || e.Item.Name == "View")
            {
                // Safely unbox the explicit data row bounded to the item template card layout container
                if (e.DataItem is ClearanceHistoryViewModel boundCardData)
                {
                    XtraMessageBox.Show($"Loading transaction records and tracking dashboard items for sequence context: {boundCardData.PeriodName}",
                        "Context Pipeline Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Your admin target management forms logic goes here
                }
            }
        }

        /// <summary>
        /// Executed when the user clicks the Save button to register a fresh semester cycle.
        /// </summary>
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboSemester.Text) || string.IsNullOrEmpty(comboSchoolYear.Text))
            {
                XtraMessageBox.Show("Please specify both Semester and Academic Year target attributes before appending new records.", "Validation Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bool inserted = _sysRepo.CreateNewPeriod(comboSemester.Text, comboSchoolYear.Text);

                if (inserted)
                {
                    XtraMessageBox.Show($"Successfully launched and archived a new processing target period context!",
                        "System State Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Re-sync template list values seamlessly from database record additions
                    LoadCurrentSystemSettings();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Pipeline execution failed during database persistence lifecycle operations: {ex.Message}", "Processing Failure");
            }
        }
    }
}