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

            // Toggle password visibility
            txtPassword.Properties.UseSystemPasswordChar = true;
            chkShowPassword.Properties.Caption = "Show Password";
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            cbRole.SelectedIndexChanged -= ToggleFieldsBasedOnRole;
            cbProgram.Items.Clear();
            cbProgram.Items.Add("BSIT");

            // Polymorphic UI — Register vs Edit mode
            this.Text = IsEdit ? "Edit Account Information" : "Register New Account";
            lblTitle.Text = IsEdit ? "Edit Information" : "Register Account";
            btnSave.Text = IsEdit ? "Update Changes" : "Save Account";
            txtUserID.ReadOnly = IsEdit;
            cbRole.Enabled = !IsEdit;

            // Pre-fill fields from model
            // In Edit mode: split FullName back into parts if the new columns
            // are empty (handles legacy records that only have FullName stored)
            txtUserID.Text = _selectedUser.UserID;
            txtLastName.Text = _selectedUser.LastName ?? string.Empty;
            txtFirstName.Text = _selectedUser.FirstName ?? string.Empty;
            txtMiddleName.Text = _selectedUser.MiddleName ?? string.Empty;
            cbRole.Text = _selectedUser.Role;
            cbProgram.Text = _selectedUser.Program?.Trim();
            cbYear.Text = _selectedUser.Year?.Trim();
            txtPassword.Text = IsEdit ? string.Empty : _selectedUser.Password;
            txtDateCreated.Text = IsEdit ? $"Generated on {DateTime.Now.ToShortDateString()}" : "Automatically Generated";

            cbRole.SelectedIndexChanged += ToggleFieldsBasedOnRole;
            ToggleFieldsBasedOnRole(null, null);
        }

        private void ToggleFieldsBasedOnRole(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbRole.Text)) return;

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);

            cbProgram.DropDownStyle = cbYear.DropDownStyle = isStudent ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
            cbProgram.Enabled = cbYear.Enabled = isStudent;
            cbProgram.BackColor = cbYear.BackColor = isStudent ? Color.White : Color.LightGray;

            cbProgram.Text = isStudent && cbProgram.Text == "N/A" ? "" : (!isStudent ? "N/A" : cbProgram.Text);
            cbYear.Text = isStudent && cbYear.Text == "N/A" ? "" : (!isStudent ? "N/A" : cbYear.Text);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // ── Validation ───────────────────────────────────────────
            // Last name and first name are required; middle name is optional
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text) ||
                (!IsEdit && string.IsNullOrWhiteSpace(txtPassword.Text)))
            {
                XtraMessageBox.Show(
                    $"Please fill in ID, Last Name, First Name, Role{(IsEdit ? "." : ", and Password.")}",
                    "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase) &&
               (string.IsNullOrWhiteSpace(cbProgram.Text) || cbProgram.Text == "N/A" ||
                string.IsNullOrWhiteSpace(cbYear.Text) || cbYear.Text == "N/A"))
            {
                XtraMessageBox.Show("Student requires a valid Program and Year.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ── Capture Form State → Model ────────────────────────────
            if (!IsEdit) _selectedUser.UserID = txtUserID.Text.Trim();

            // Store the three name parts — FullName is computed automatically
            // from these in the User model, no need to set it manually
            _selectedUser.LastName = txtLastName.Text.Trim();
            _selectedUser.FirstName = txtFirstName.Text.Trim();
            _selectedUser.MiddleName = txtMiddleName.Text.Trim();

            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;

            if (!IsEdit || !string.IsNullOrWhiteSpace(txtPassword.Text))
                _selectedUser.Password = txtPassword.Text.Trim();

            // ── Execute Business Pipeline ─────────────────────────────
            if (!IsEdit)
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
            else if (_userRepo.EditUser(_selectedUser))
            {
                CloseWithResult(DialogResult.OK, "Account updated!");
            }
        }

        private void CloseWithResult(DialogResult res, string msg)
        {
            XtraMessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = res;
            Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtFullName_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}