using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : XtraForm
    {
        private readonly FormMode _mode;
        private readonly User _selectedUser;
        private readonly UserRepository _userRepo = new UserRepository();

        private bool IsEdit => _mode == FormMode.Edit;

        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User();

            txtPassword.Properties.UseSystemPasswordChar = true;
            ConfigurePasswordToggle();
            AttachNameRestrictions(txtLastName, txtFirstName, txtMiddleName);

            cbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProgram.DropDownStyle = ComboBoxStyle.DropDownList;
            cbYear.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // ── Setup ─────────────────────────────────────────────────────
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

        private void AttachNameRestrictions(params DevExpress.XtraEditors.TextEdit[] fields)
        {
            foreach (var f in fields) f.KeyPress += RestrictToLettersOnly;
        }

        private void RestrictToLettersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
                e.Handled = true;
        }

        // ── Load ──────────────────────────────────────────────────────
        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            cbRole.SelectedIndexChanged -= ToggleFieldsBasedOnRole;

            cbProgram.Items.Clear();
            cbProgram.Items.Add("BSIT");

            this.Text = IsEdit ? "Edit Account Information" : "Register New Account";
            lblTitle.Text = IsEdit ? "Edit Information" : "Register Account";
            btnSave.Text = IsEdit ? "Update Changes" : "Save Account";
            txtUserID.ReadOnly = IsEdit;
            cbRole.Enabled = !IsEdit;

            PopulateFields();

            cbRole.SelectedIndexChanged += ToggleFieldsBasedOnRole;
            ToggleFieldsBasedOnRole(null, null);
        }

        private void PopulateFields()
        {
            txtUserID.Text = _selectedUser.UserID;
            txtLastName.Text = _selectedUser.LastName ?? string.Empty;
            txtFirstName.Text = _selectedUser.FirstName ?? string.Empty;
            txtMiddleName.Text = _selectedUser.MiddleName ?? string.Empty;
            cbRole.Text = _selectedUser.Role;
            cbProgram.Text = _selectedUser.Program?.Trim();
            cbYear.Text = _selectedUser.Year?.Trim();
            txtPassword.Text = IsEdit ? string.Empty : _selectedUser.Password;
            txtDateCreated.Text = IsEdit
                ? $"Generated on {DateTime.Now.ToShortDateString()}"
                : "Automatically Generated";
        }

        // ── Role Toggle ───────────────────────────────────────────────
        private void ToggleFieldsBasedOnRole(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbRole.Text)) return;

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);
            cbProgram.Enabled = cbYear.Enabled = isStudent;

            var bg = isStudent ? Color.White : Color.LightGray;
            cbProgram.BackColor = cbYear.BackColor = bg;

            if (!isStudent)
            {
                cbProgram.Text = cbYear.Text = "N/A";
            }
            else if (cbProgram.Text == "N/A") cbProgram.Text = string.Empty;
            else if (cbYear.Text == "N/A") cbYear.Text = string.Empty;
        }

        // ── Save ──────────────────────────────────────────────────────
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!ValidateFields()) return;

            ApplyFormDataToUser();

            if (!IsEdit)
                SaveNewUser();
            else
                UpdateExistingUser();
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text) ||
                (!IsEdit && string.IsNullOrWhiteSpace(txtPassword.Text)))
            {
                XtraMessageBox.Show(
                    $"Please fill in ID, Last Name, First Name, Role{(IsEdit ? "." : ", and Password.")}",
                    "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool studentNeedsProgram =
                cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrWhiteSpace(cbProgram.Text) || cbProgram.Text == "N/A" ||
                 string.IsNullOrWhiteSpace(cbYear.Text) || cbYear.Text == "N/A");

            if (studentNeedsProgram)
            {
                XtraMessageBox.Show("Student requires a valid Program and Year.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ApplyFormDataToUser()
        {
            if (!IsEdit) _selectedUser.UserID = txtUserID.Text.Trim();

            _selectedUser.LastName = txtLastName.Text.Trim();
            _selectedUser.FirstName = txtFirstName.Text.Trim();
            _selectedUser.MiddleName = txtMiddleName.Text.Trim();
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;

            if (!IsEdit || !string.IsNullOrWhiteSpace(txtPassword.Text))
                _selectedUser.Password = Helpers.PasswordHelper.Hash(txtPassword.Text.Trim());
        }

        private void SaveNewUser()
        {
            if (_userRepo.AddUser(_selectedUser))
                CloseWithResult(DialogResult.OK, "Registration Successful!");
            else
            {
                XtraMessageBox.Show($"User ID '{_selectedUser.UserID}' is taken.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
            }
        }

        private void UpdateExistingUser()
        {
            if (_userRepo.EditUser(_selectedUser))
                CloseWithResult(DialogResult.OK, "Account updated!");
        }

        private void CloseWithResult(DialogResult result, string message)
        {
            XtraMessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = result;
            Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtFullName_EditValueChanged(object sender, EventArgs e) { }
    }
}