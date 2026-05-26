using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class Registration : XtraForm
    {
        // Allows injecting a mock repo for testing; defaults to a real one
        private readonly UserRepository _userRepo;

        public Registration(UserRepository userRepo = null)
        {
            InitializeComponent();
            _userRepo = userRepo ?? new UserRepository();
            ConfigurePasswordToggle();

            // Restrict name fields to letters, spaces, hyphens, and apostrophes only
            AttachNameRestrictions(txtLastName, txtFirstName, txtMiddleName);
        }

        // ── Setup ─────────────────────────────────────────────────────
        private void ConfigurePasswordToggle()
        {
            // Toggles password masking and updates the checkbox label on every check change
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
            };
        }

        // Loops through each field and hooks the same key filter to all of them
        private void AttachNameRestrictions(params DevExpress.XtraEditors.TextEdit[] fields)
        {
            foreach (var field in fields)
                field.KeyPress += RestrictToLettersOnly;
        }

        // Blocks any key that isn't a letter, space, hyphen, or apostrophe
        private void RestrictToLettersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
                e.Handled = true;
        }

        // ── Register ──────────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            if (!ValidateRequiredFields()) return;

            // Build a User object from the form fields and attempt to save it
            var newUser = BuildUserFromForm();

            if (_userRepo.AddUser(newUser))
            {
                // Registration succeeded — clear the form for the next entry
                XtraMessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                // AddUser returns false when the UserID already exists in the database
                XtraMessageBox.Show($"User ID '{newUser.UserID}' is already taken. Choose another.",
                    "Duplicate User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
            }
        }

        // Returns false and shows a message if any required field is empty
        private bool ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                XtraMessageBox.Show("User ID, Last Name, First Name, and Password are required.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbProgram.Text) || string.IsNullOrWhiteSpace(cmbYear.Text))
            {
                XtraMessageBox.Show("Please select a Program and Year.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        // Collects all form field values into a new User object — role is always Student here
        private User BuildUserFromForm() => new User
        {
            UserID = txtUserID.Text.Trim(),
            LastName = txtLastName.Text.Trim(),
            FirstName = txtFirstName.Text.Trim(),
            MiddleName = txtMiddleName.Text.Trim(),
            Program = cmbProgram.Text,
            Year = cmbYear.Text,
            Role = "Student",
            Password = txtPassword.Text
        };

        // Resets all input fields back to empty after a successful registration
        private void ClearFields()
        {
            txtUserID.Text = txtLastName.Text = txtFirstName.Text =
            txtMiddleName.Text = txtPassword.Text = string.Empty;
            cmbProgram.EditValue = cmbYear.EditValue = null;
        }

        // ── Navigate to Login ─────────────────────────────────────────
        private void lblctrLogin_Click(object sender, EventArgs e)
        {
            var loginForm = new Login();

            // Close registration when the login form is closed
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }
    }
}