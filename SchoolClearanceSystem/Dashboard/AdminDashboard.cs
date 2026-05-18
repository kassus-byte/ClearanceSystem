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
    }
}