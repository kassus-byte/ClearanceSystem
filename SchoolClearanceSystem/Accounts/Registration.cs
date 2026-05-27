using DevExpress.XtraEditors;
using SchoolClearanceSystem.Helpers;
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

            // Delegate password toggle and name restrictions to UIHelper
            UIHelper.ConfigurePasswordToggle(chkShowPassword, txtPassword);
            UIHelper.AttachNameRestrictions(txtLastName, txtFirstName, txtMiddleName);
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
                UIHelper.ShowSuccess("Registration Successful!");
                ClearFields();
            }
            else
            {
                // AddUser returns false when the UserID already exists in the database
                UIHelper.ShowError($"User ID '{newUser.UserID}' is already taken. Choose another.", "Duplicate User ID");
                txtUserID.Focus();
            }
        }

        // Returns false and shows a message if any required field is empty
        private bool ValidateRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtMiddleName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                UIHelper.ShowWarning("User ID, Last Name, First Name, Middle Initial, and Password are required.", "Validation Error");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbProgram.Text) || string.IsNullOrWhiteSpace(cmbYear.Text))
            {
                UIHelper.ShowWarning("Please select a Program and Year.", "Validation Error");
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