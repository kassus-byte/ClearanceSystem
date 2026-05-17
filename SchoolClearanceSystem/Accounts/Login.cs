// Login.cs
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

            // Hide password on startup
            txtPassword.Properties.UseSystemPasswordChar = true;

            // Wire CheckEdit using Properties event — works for ALL DevExpress CheckEdit
            // Replace "chkShowPassword" below with whatever your CheckEdit (Name) is
            chkShowPassword.Properties.Caption = "Show Password";
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };
        }

        // ── Login button ─────────────────────────────────────────────────
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
                User user = Session.CurrentUser;

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
                    case "Technical":
                        nextForm = new TechnicalOffice();
                        break;
                    case "Student":
                        nextForm = new StudentPortal();
                        break;
                    default:
                        XtraMessageBox.Show("Role not recognized. Contact Admin.",
                            "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }

                nextForm.FormClosed += (s, args) => this.Close();
                nextForm.Show();
                this.Hide();
            }
            else
            {
                XtraMessageBox.Show("Invalid UserID or Password.", "Login Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        // ── Register link ─────────────────────────────────────────────────
       
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