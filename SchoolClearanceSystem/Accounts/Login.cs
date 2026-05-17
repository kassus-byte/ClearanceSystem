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

            // Set initial label text on startup
            chkShowPassword.Properties.Caption = "Show Password";

            // Wire CheckEdit event to change both password visibility and the label text
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                // 1. Toggle password visibility
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;

                // 2. Dynamically change the text based on checked state
                if (chkShowPassword.Checked)
                {
                    chkShowPassword.Properties.Caption = "Hide Password";
                }
                else
                {
                    chkShowPassword.Properties.Caption = "Show Password";
                }

                // Keep focus and put cursor at the end of the text
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