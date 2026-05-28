using DevExpress.XtraEditors;
using SchoolClearanceSystem.Dashboard;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class Login : XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();
        private readonly Dictionary<string, Func<Form>> _roleForms;

        public Login()
        {
            InitializeComponent();
            ConfigurePasswordToggle();
            _roleForms = new Dictionary<string, Func<Form>>
            {
                { "Admin",            () => new AdminDashboard()    },
                { "Treasurer",        () => new TreasurerDashboard() },
                { "Technical Office", () => new TechnicalOffice()   },
                { "SSG",              () => new SSGOffice()          },
                { "Student",          () => new StudentPortal()      },
            };
        }
        private void ConfigurePasswordToggle()
        {
            chkShowPassword.Properties.Caption = "Show Password";
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";

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
                Notify("Please enter both ID and Password.", "Validation Error", MessageBoxIcon.Warning);
                return;
            }

            if (_userRepo.ValidateLogin(id, pass) == null)
            {
                Notify("Invalid User ID or Password.", "Login Failed", MessageBoxIcon.Error);
                return;
            }

            Session.CurrentUser = _userRepo.GetUserDetails(id);
            var user = Session.CurrentUser;
            if (user == null) return;

            if (!_roleForms.TryGetValue(user.Role, out var createForm))
            {
                Notify("Invalid User Role detected.", "Access Denied", MessageBoxIcon.Error);
                return;
            }

            createForm().Show();
            this.Hide();
        }
        private void lnkRegister_Click(object sender, EventArgs e)
        {
            var reg = new Registration();
            reg.FormClosed += (s, args) => this.Show();
            reg.Show();
            this.Hide();
        }
        private void Notify(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Information) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.OK, icon);

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}