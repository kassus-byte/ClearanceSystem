using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();

        public Registration()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();

            // Password visibility toggle
            txtPassword.Properties.UseSystemPasswordChar = true;
            chkShowPassword.Properties.Caption = "Show Password";
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };

            // ── Combo box restrictions ────────────────────────────────
            // Program and Year must be selected from the list — no free typing
            cmbProgram.Properties.Items.Clear();
            cmbProgram.Properties.Items.AddRange(new object[] { "BSIT" });
            cmbProgram.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cmbYear.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            // ── Name field restrictions ───────────────────────────────
            // Letters, spaces, hyphens, and apostrophes only — no digits
            txtLastName.KeyPress += RestrictToLettersOnly;
            txtFirstName.KeyPress += RestrictToLettersOnly;
            txtMiddleName.KeyPress += RestrictToLettersOnly;
        }

        // Allows letters (any language), spaces, hyphens, and apostrophes.
        // Blocks digits and every other symbol.
        private void RestrictToLettersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' &&
                e.KeyChar != '-' &&
                e.KeyChar != '\'')
            {
                e.Handled = true;
            }
        }

        // ── Register button ───────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                XtraMessageBox.Show("User ID, Last Name, First Name, and Password are required.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbProgram.Text) || string.IsNullOrWhiteSpace(cmbYear.Text))
            {
                XtraMessageBox.Show("Please select a Program and Year.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User newUser = new User
            {
                UserID = txtUserID.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                FirstName = txtFirstName.Text.Trim(),
                MiddleName = txtMiddleName.Text.Trim(),
                Program = cmbProgram.Text,
                Year = cmbYear.Text,
                Role = "Student",
                Password = txtPassword.Text.Trim(),
            };

            if (_userRepo.AddUser(newUser))
            {
                XtraMessageBox.Show("Registration Successful!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                XtraMessageBox.Show($"User ID '{newUser.UserID}' is already taken. Choose another.",
                    "Duplicate User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
            }
        }

        // ── Clear all fields ──────────────────────────────────────────────
        private void ClearFields()
        {
            txtUserID.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            cmbProgram.EditValue = null;
            cmbYear.EditValue = null;
        }

        // ── Navigate to Login ─────────────────────────────────────────────
        private void lblctrLogin_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }

        private void labelControl8_Click(object sender, EventArgs e) { }
        private void labelControl7_Click(object sender, EventArgs e) { }
    }
}