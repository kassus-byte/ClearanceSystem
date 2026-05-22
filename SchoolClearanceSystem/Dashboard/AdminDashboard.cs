using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();
            SetupGridBehaviors();
            LoadCurrentSystemSettings(); // Dynamic Rendering: Build the history log on startup
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
                XtraMessageBox.Show($"Database tracking dependency error: {ex.Message}", "Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearanceSystem_Click(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageClearanceSystem;
            RefreshData();
        }

        // ── Chronological Clearance History Rendering Engine ─────────────────────────

        /// <summary>
        /// Reads all historical configuration blocks from the database layer and builds UI cards dynamically.
        /// </summary>
        private void LoadCurrentSystemSettings()
        {
            try
            {
                // Clear any existing dynamically generated controls inside your layout panel container
                flowPeriodHistory.Controls.Clear();

                var historicalPeriods = _sysRepo.GetAllPeriods();

                foreach (var period in historicalPeriods)
                {
                    // Explicitly cast PeriodID to (int) to prevent type deduction crashes
                    int safePeriodId = Convert.ToInt32(period.PeriodID);

                    // Call our UI rendering engine factory method with matching argument structures
                    var periodCard = CreatePeriodCardControl(safePeriodId, period.Semester?.ToString(), period.AcademicYear?.ToString(), period.IsActive == 1);
                    flowPeriodHistory.Controls.Add(periodCard);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Failed to load operational clearance history records: {ex.Message}", "UI Render Error");
            }
        }

        /// <summary>
        /// UI Control Builder Factory: Programmatically constructs custom, isolated display blocks matching your prototype image layout
        /// </summary>
        private PanelControl CreatePeriodCardControl(int periodId, string semester, string schoolYear, bool isActive)
        {
            // 1. Setup Base Card Control Panel Box Container Frame
            PanelControl card = new PanelControl();
            card.Size = new System.Drawing.Size(940, 60);
            card.Margin = new Padding(0, 5, 0, 5);
            card.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;

            // 2. Add Informational context label
            LabelControl lblInfo = new LabelControl();
            lblInfo.Text = $"ℹ️  {schoolYear} {semester}";
            lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            lblInfo.Location = new System.Drawing.Point(20, 20);

            // 3. Setup Actions Trigger Button Controls
            SimpleButton btnAction = new SimpleButton();
            btnAction.Text = "View";
            btnAction.Size = new System.Drawing.Size(90, 30);
            btnAction.Location = new System.Drawing.Point(260, 15);
            btnAction.StyleController = null;
            btnAction.Appearance.BackColor = System.Drawing.Color.Navy;
            btnAction.Appearance.ForeColor = System.Drawing.Color.White;
            btnAction.Click += (s, e) => {
                XtraMessageBox.Show($"Loading student transactional metrics for historical cycle registry ID: {periodId}", "Context Loaded");
            };

            // 4. State Message Flag badge
            LabelControl lblStatusBadge = new LabelControl();
            lblStatusBadge.Text = isActive ? "Clearance Processing Active" : "Clearance Done";
            lblStatusBadge.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            lblStatusBadge.ForeColor = isActive ? System.Drawing.Color.OrangeRed : System.Drawing.Color.ForestGreen;
            lblStatusBadge.Location = new System.Drawing.Point(550, 18);

            // Assemble UI Component Tree safely
            card.Controls.Add(lblInfo);
            card.Controls.Add(btnAction);
            card.Controls.Add(lblStatusBadge);

            return card;
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
                // Appends a brand new row entry tracking block into the database architecture
                bool inserted = _sysRepo.CreateNewPeriod(comboSemester.Text, comboSchoolYear.Text);

                if (inserted)
                {
                    XtraMessageBox.Show($"Successfully launched and archived a new processing target period context!",
                        "System State Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Re-render layout panels straight from the DB state record updates seamlessly
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