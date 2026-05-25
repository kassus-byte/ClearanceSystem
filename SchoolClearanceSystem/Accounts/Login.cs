using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using SchoolClearanceSystem.Dashboard;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();

        public Login()
        {
            InitializeComponent();

            chkShowPassword.Properties.Caption = "Show Password";

            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;

               
                if (chkShowPassword.Checked)
                {
                    chkShowPassword.Properties.Caption = "Hide Password";
                }
                else
                {
                    chkShowPassword.Properties.Caption = "Show Password";
                }

              
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };
        }

       
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtUserID.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Please enter both ID and Password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var loggedInUser = _userRepo.ValidateLogin(id, pass);

            if (loggedInUser != null)
            {
                Session.CurrentUser = _userRepo.GetUserDetails(id);
                var user = Session.CurrentUser;

                if (user == null) return;

                Form nextForm = null;

                switch (user.Role)
                {
                    case "Admin":
                        nextForm = new AdminDashboard();
                        break;
                    case "Treasurer":
                        nextForm = new TreasurerDashboard();
                        break;
                    case "Technical Office":
                        nextForm = new TechnicalOffice();
                        break;
                    case "SSG":
                        nextForm = new SSGOffice();
                        break;
                    case "Student":
                        nextForm = new StudentPortal();
                        break;
                    default:
                        DevExpress.XtraEditors.XtraMessageBox.Show("Invalid User Role detected.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                if (nextForm != null)
                {
                    nextForm.Show();
                    this.Hide();
                }
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Invalid User ID or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e) { }

        private void lnkRegister_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            reg.FormClosed += (s, args) => this.Show();
            reg.Show();
            this.Hide();
        }
    }
}