using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        DatabaseManager db = new DatabaseManager();

        public AdminDashboard()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
        {
            gcStudents.DataSource = db.GetUsersByRole("Student", true);
            gcOffice.DataSource = db.GetUsersByRole("", false);
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (gvStudents.GetFocusedRow() is User selectedUser)
            {
                try
                {
                DocumentService.ViewDocument(selectedUser.UploadPath);
                }
                catch (Exception ex)
                {
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
            db.ToggleClearanceSeason(tsStatus.IsOn);

            string status = tsStatus.IsOn ? "OPEN" : "CLOSED";
            XtraMessageBox.Show($"Clearance season is now {status}.", "System Update",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRegisterAccount_Click(object sender, EventArgs e)
        {
            using (UserInfoForm frm = new UserInfoForm(FormMode.Register, null))
            {
                frm.StartPosition = FormStartPosition.CenterParent;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshData();
                }
            }
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            var activeView = gvStudents.IsFocusedView ? gvStudents : gvOffice;
 
            if (activeView.GetFocusedRow() is User selectedUser)
            {
                using (UserInfoForm frm = new UserInfoForm(FormMode.Edit, selectedUser))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        RefreshData();
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