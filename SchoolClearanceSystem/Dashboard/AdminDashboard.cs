using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;       // To recognize the User class
using SchoolClearanceSystem.Repository;   // To recognize UserRepository and SystemRepository

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        // OOP: Encapsulation - The dashboard uses specialized workers (repositories)
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();

            // Set the initial state of the toggle based on the database
            tsStatus.IsOn = _sysRepo.IsClearanceActive();
        }

        private void RefreshData()
        {
            // GridControl Students
            gcStudents.DataSource = _userRepo.GetUsersByRole("Student", true);

            // GridControl Offices (Roles that aren't Student or Admin)
            gcOffice.DataSource = _userRepo.GetUsersByRole("", false);
        }

        private void tsStatus_Toggled(object sender, EventArgs e)
        {
            // Save the toggle state to the database via SystemRepository
            _sysRepo.ToggleClearanceSeason(tsStatus.IsOn);

            string status = tsStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance season is now {status}.", "System Update",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            // Open UserInfoForm in Register mode
            using (UserInfoForm frm = new UserInfoForm(FormMode.Register, null))
            {
                frm.StartPosition = FormStartPosition.CenterParent;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData(); // Reload grids if a new user was added
                }
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            // Identify which grid is currently active/focused
            var activeView = gvStudents.IsFocusedView ? gvStudents : gvOffice;

            if (activeView.GetFocusedRow() is User selectedUser)
            {
                // Open UserInfoForm in Edit mode with the selected user object
                using (UserInfoForm frm = new UserInfoForm(FormMode.Edit, selectedUser))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshData(); // Reload grids to show updated info
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Please select an account from the list to edit.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            // Document Viewing logic
            if (gvStudents.GetFocusedRow() is User selectedUser)
            {
                try
                {
                    if (!string.IsNullOrEmpty(selectedUser.UploadPath))
                    {
                        DocumentService.ViewDocument(selectedUser.UploadPath);
                    }
                    else
                    {
                        XtraMessageBox.Show("No document uploaded for this student.", "Not Found");
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- NAVIGATION LOGIC ---

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageDashboard;
        }

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close(); // Or return to Login Form
        }
    }
}