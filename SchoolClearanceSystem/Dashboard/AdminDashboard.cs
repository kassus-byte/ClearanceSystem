using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        // Global instance of your Database Manager
        DatabaseManager db = new DatabaseManager();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            // OOP: Assigning Lists of User objects directly to the GridControls
            // The DatabaseManager now handles the SQL logic internally
            gcStudents.DataSource = db.GetUsersByRole("Student", true);
            gcOffice.DataSource = db.GetUsersByRole("", false);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            // 1. Get the User object from the row (using 'as' for safety)
            if (gvStudents.GetFocusedRow() is User selectedUser)
            {
                try
                {
                    // 2. Call the DocumentService to handle the file opening
                    // This is 'Encapsulation' - the Dashboard doesn't need to know HOW to open a file
                    DocumentService.ViewDocument(selectedUser.UploadPath);
                }
                catch (Exception ex)
                {
                    // Catch the error thrown by DocumentService if the file is missing
                    XtraMessageBox.Show(ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDashboard_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageDashboard;
        }

        private void btnAccountManagement_Click_1(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;
        }

        private void tsStatus_Toggled(object sender, EventArgs e)
        {
            // Update the system setting in the database
            db.ToggleClearanceSeason(tsStatus.IsOn);

            string status = tsStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance season is now {status}.", "System Update",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            // Open the form in Register mode. Passing 'null' because there is no existing user data yet.
            using (UserInfoForm frm = new UserInfoForm(FormMode.Register, null))
            {
                frm.StartPosition = FormStartPosition.CenterParent;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData(); // Reload the grids to show the new account
                }
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            // OOP Trick: Check which GridView is currently being looked at by the user
            var activeView = gvStudents.IsFocusedView ? gvStudents : gvOffice;

            // Cast the focused row directly to our User object
            if (activeView.GetFocusedRow() is User selectedUser)
            {
                using (UserInfoForm frm = new UserInfoForm(FormMode.Edit, selectedUser))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshData(); // Reload grids to reflect changes
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Please select an account from the list to edit.", "Selection Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}