using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;       // Added: Namespace for User class
using SchoolClearanceSystem.Repository;   // Added: Namespace for UserRepository

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    // OOP Inheritance: If you created a BaseForm, change XtraForm to BaseOfficeForm
    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private FormMode _mode;
        private User _selectedUser;

        // REFACTORED: Use the Repository instead of the DatabaseManager
        private UserRepository _userRepo = new UserRepository();

        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User();
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            SetupForm();
        }

        private void SetupForm()
        {
            if (_mode == FormMode.Edit)
            {
                this.Text = "Edit Account Information";
                btnSave.Text = "Update Changes";

                txtUserID.Text = _selectedUser.UserID;
                txtUserID.ReadOnly = true;
                txtFullName.Text = _selectedUser.FullName;
                cbProgram.Text = _selectedUser.Program;
                cbYear.Text = _selectedUser.Year;
                cbRole.Text = _selectedUser.Role;

                // Improved null check for DateCreated
                txtDateCreated.Text = !string.IsNullOrEmpty(_selectedUser.DateCreated)
                                      ? _selectedUser.DateCreated
                                      : "N/A";
                txtDateCreated.Visible = true;
            }
            else
            {
                this.Text = "Register New Account";
                btnSave.Text = "Save Account";
                txtUserID.ReadOnly = false;
                txtDateCreated.Text = "Automatically Generated";
            }
        }

        private void PerformRegister()
        {
            _selectedUser.UserID = txtUserID.Text;
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Password = "123";

            // REFACTORED: Call the Repository
            if (_userRepo.SaveUser(_selectedUser))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void PerformUpdate()
        {
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Role = cbRole.Text;

            // REFACTORED: Call the Repository
            if (_userRepo.UpdateUser(_selectedUser))
            {
                XtraMessageBox.Show("Information successfully updated!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                XtraMessageBox.Show("Please fill in all required fields.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_mode == FormMode.Register) PerformRegister();
            else PerformUpdate();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}