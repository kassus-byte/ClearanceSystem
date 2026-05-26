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
        // Handles all user account queries (validate, fetch details)
        private readonly UserRepository _userRepo = new UserRepository();

        // Maps each role string to the Form that should open — no switch needed
        private readonly Dictionary<string, Func<Form>> _roleForms;

        public Login()
        {
            InitializeComponent();
            ConfigurePasswordToggle();

            // Each role key points to a lambda that creates the correct dashboard
            _roleForms = new Dictionary<string, Func<Form>>
            {
                { "Admin",            () => new AdminDashboard()    },
                { "Treasurer",        () => new TreasurerDashboard() },
                { "Technical Office", () => new TechnicalOffice()   },
                { "SSG",              () => new SSGOffice()          },
                { "Student",          () => new StudentPortal()      },
            };
        }

        // ── Password Toggle ───────────────────────────────────────────
        private void ConfigurePasswordToggle()
        {
            chkShowPassword.Properties.Caption = "Show Password";

            // Toggles password masking and updates the checkbox label on every check change
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";

                // Keep cursor at end of text after toggle
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };
        }

        // ── Login ─────────────────────────────────────────────────────
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtUserID.Text.Trim();
            string pass = txtPassword.Text;

            // Block empty submissions before hitting the database
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                Notify("Please enter both ID and Password.", "Validation Error", MessageBoxIcon.Warning);
                return;
            }

            // Check credentials against the database (BCrypt verified inside)
            var loggedInUser = _userRepo.ValidateLogin(id, pass);
            if (loggedInUser == null)
            {
                Notify("Invalid User ID or Password.", "Login Failed", MessageBoxIcon.Error);
                return;
            }

            // Store full user details in the global session for use across all forms
            Session.CurrentUser = _userRepo.GetUserDetails(id);
            var user = Session.CurrentUser;
            if (user == null) return;

            // Look up which form to open based on the user's role
            if (!_roleForms.TryGetValue(user.Role, out var createForm))
            {
                Notify("Invalid User Role detected.", "Access Denied", MessageBoxIcon.Error);
                return;
            }

            // Open the role-appropriate dashboard and hide the login form
            var nextForm = createForm();
            nextForm.Show();
            this.Hide();
        }

        // ── Register Link ─────────────────────────────────────────────
        private void lnkRegister_Click(object sender, EventArgs e)
        {
            var reg = new Registration();

            // Bring login back when registration window closes
            reg.FormClosed += (s, args) => this.Show();
            reg.Show();
            this.Hide();
        }

        private void panelControl1_Paint(object sender, System.Drawing.Graphics e) { }

        // ── Helper ────────────────────────────────────────────────────
        // Shorthand for showing a message box — keeps other methods clean
        private void Notify(string text, string title, MessageBoxIcon icon = MessageBoxIcon.Information) =>
            XtraMessageBox.Show(text, title, MessageBoxButtons.OK, icon);
    }
}