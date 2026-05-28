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
        // OOP CONCEPT: DEPENDENCY INJECTION (Allows looser coupling for testing mocks)
        private readonly UserRepository _userRepo;

        public Registration(UserRepository userRepo = null)
        {
            InitializeComponent();
            _userRepo = userRepo ?? new UserRepository();

            UIHelper.ConfigurePasswordToggle(chkShowPassword, txtPassword);
            UIHelper.AttachNameRestrictions(txtLastName, txtFirstName, txtMiddleName);
        }

        // ── Register ──────────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            if (!ValidateRequiredFields()) return;

            // OOP CONCEPT: OBJECT INITIALIZATION / ENCAPSULATION
            var newUser = BuildUserFromForm();

            if (_userRepo.AddUser(newUser))
            {
                UIHelper.ShowSuccess("Registration Successful!");
                ClearFields();
            }
            else
            {
                UIHelper.ShowError($"User ID '{newUser.UserID}' is already taken. Choose another.", "Duplicate User ID");
                txtUserID.Focus();
            }
        }

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

        private User BuildUserFromForm() => new User
        {
            UserID = txtUserID.Text.Trim(),
            LastName = txtLastName.Text.Trim(),
            FirstName = txtFirstName.Text.Trim(),
            MiddleName = txtMiddleName.Text.Trim(),
            Program = cmbProgram.Text,
            Year = cmbYear.Text,
            Role = "Student",
            Password = Helpers.PasswordHelper.Hash(txtPassword.Text)
        };

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
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }
    }
}