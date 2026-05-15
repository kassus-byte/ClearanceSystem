using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using SchoolClearanceSystem.Dashboard; 
namespace SchoolClearanceSystem
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtUserID.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                XtraMessageBox.Show("Please enter both ID and Password.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_userRepo.ValidateLogin(id, pass))
            {
                Session.CurrentUser = _userRepo.GetUserDetails(id);
                var user = Session.CurrentUser;

                if (user != null)
                {
                    Form nextForm = null;

                    if (user.Role == "Admin")
                    {
                        nextForm = new AdminDashboard();
                    }
                    else if (user.Role == "Treasurer")
                    {
                        nextForm = new TreasurerDashboard();
                    }
                    else if (user.Role == "Technical")
                    {
                        nextForm = new TechnicalOffice();
                    }
                    else if (user.Role == "Student")
                    {
                        nextForm = new StudentPortal();
                    }
                    else
                    {
                        XtraMessageBox.Show("Your role is not recognized. Contact Admin.", "Access Denied");
                        return;
                    }

                    if (nextForm != null)
                    {
                        nextForm.FormClosed += (s, args) => this.Close();
                        nextForm.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Invalid UserID or Password.", "Login Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}