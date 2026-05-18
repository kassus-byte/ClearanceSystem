using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.ComponentModel;
using System.Linq;
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

        /// <summary>
        /// Feeds correct criteria filters to fill both grid views accurately.
        /// </summary>
        private void RefreshData()
        {
            // Binds data source to Student collections
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student");

            // Binds data source to all administrative staff roles (Admin, Treasurer, Technical Office, etc.)
            gcOffice.DataSource = _userRepo.GetUsersByRole("Staff");
        }

        private void SetupGridBehaviors()
        {
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
            using (UserInfoForm frm = new UserInfoForm(FormMode.Register, null))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                if (frm.ShowDialog(this) == DialogResult.OK) RefreshData();
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            var activeView = (tabPane1.SelectedPage.Caption == "Students") ? gvStudents : gvOffice;

            if (activeView.FocusedRowHandle >= 0 && activeView.GetFocusedRow() is User selectedUser)
            {
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
            DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(
               "Are you sure you want to logout?",
               "Logout",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }
    }
}