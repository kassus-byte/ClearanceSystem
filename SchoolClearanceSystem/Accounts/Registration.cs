using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo;

        public Registration(UserRepository userRepo = null)
        {
            InitializeComponent();
            _userRepo = userRepo ?? new UserRepository();
            ConfigurePasswordToggle();
        }

        private void ConfigurePasswordToggle()
        {
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
            };

            txtLastName.KeyPress += RestrictToLettersOnly;
            txtFirstName.KeyPress += RestrictToLettersOnly;
            txtMiddleName.KeyPress += RestrictToLettersOnly;
        }

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
                Password = txtPassword.Text
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

        private void lblctrLogin_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }
    }
}