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
                // --- POLYMORPHIC BEHAVIOR: EDIT MODE ---
                this.Text = "Edit Account Information";
                lblTitle.Text = "Edit Information"; // Change the big label at the top
                btnSave.Text = "Update Changes";

                // Fill data
                txtUserID.Text = _selectedUser.UserID;
                txtFullName.Text = _selectedUser.FullName;
                cbProgram.Text = _selectedUser.Program;
                cbYear.Text = _selectedUser.Year;
                cbRole.Text = _selectedUser.Role;

                // --- REQUIREMENT: Disable Year and Program during Edit ---
                txtUserID.ReadOnly = true;

                // Standard WinForms ComboBox uses .Enabled instead of .ReadOnly
                cbProgram.Enabled = false;
                cbYear.Enabled = false;

                // To make it look "greyed out" but readable for standard controls:
                cbProgram.BackColor = System.Drawing.Color.LightGray;
                cbYear.BackColor = System.Drawing.Color.LightGray;
            }
            else
            {
                // --- POLYMORPHIC BEHAVIOR: REGISTER MODE ---
                this.Text = "Register New Account";
                lblTitle.Text = "Register Account";
                btnSave.Text = "Save Account";

                // Enable everything for a new student
                txtUserID.ReadOnly = false;
                cbProgram.Enabled = true;
                cbYear.Enabled = true;
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
            // 1. DYNAMIC VALIDATION
            if (_mode == FormMode.Register)
            {
                // Check ALL fields for Registration
                if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                    string.IsNullOrWhiteSpace(txtFullName.Text) ||
                    string.IsNullOrWhiteSpace(cbProgram.Text) ||
                    string.IsNullOrWhiteSpace(cbYear.Text) ||
                    string.IsNullOrWhiteSpace(cbRole.Text))
                {
                    XtraMessageBox.Show("All fields must be filled for registration.", "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // Only check name for Edit (since others are disabled/read-only)
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    XtraMessageBox.Show("Name cannot be empty.", "Validation Error");
                    return;
                }
            }

            // 2. PROCEED TO REPOSITORY
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