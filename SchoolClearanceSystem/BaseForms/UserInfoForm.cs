using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System.Drawing;

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private FormMode _mode;
        private User _selectedUser;
        private UserRepository _userRepo = new UserRepository();

        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User();

            // This links the event: "When selection changes, run the toggle logic"
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            SetupForm();
        }

        private void SetupForm()
        {
            txtUserID.Text = _selectedUser.UserID;
            txtFullName.Text = _selectedUser.FullName;
            cbProgram.Text = _selectedUser.Program;
            cbYear.Text = _selectedUser.Year;
            cbRole.Text = _selectedUser.Role;
            txtDateCreated.Text = _mode == FormMode.Edit ? "Generated on " + DateTime.Now.ToShortDateString() : "Automatically Generated";

            if (!string.IsNullOrEmpty(_selectedUser.UploadPath))
            {
                try
                {
                    if (System.IO.File.Exists(_selectedUser.UploadPath))
                    {
                        pePhoto.Image = Image.FromFile(_selectedUser.UploadPath);
                    }
                }
                catch (Exception) { pePhoto.Image = null; }
            }

            if (_mode == FormMode.Edit)
            {
                this.Text = "Edit Account Information";
                lblTitle.Text = "Edit Information";
                btnSave.Text = "Update Changes";
                txtUserID.ReadOnly = true;
                cbRole.Enabled = false;
            }
            else
            {
                this.Text = "Register New Account";
                lblTitle.Text = "Register Account";
                btnSave.Text = "Save Account";
                txtUserID.ReadOnly = false;
                cbRole.Enabled = true;
            }

            // Run this once on load to handle pre-filled data (like in Edit mode)
            ToggleFieldsBasedOnRole();
        }

        // This triggers EVERY TIME a user picks a different role in the dropdown
        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleFieldsBasedOnRole();
        }

        private void ToggleFieldsBasedOnRole()
        {
            // Logic: If NOTHING is selected yet, keep them enabled.
            // If "Student" is selected, keep them enabled.
            // If ANYTHING ELSE (Admin, Treasurer, Dean, Technical Office) is selected, disable them.

            if (string.IsNullOrWhiteSpace(cbRole.Text))
            {
                return; // Do nothing if the role is blank
            }

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);

            if (isStudent)
            {
                cbProgram.Enabled = true;
                cbYear.Enabled = true;
                cbProgram.BackColor = Color.White;
                cbYear.BackColor = Color.White;

                if (cbProgram.Text == "N/A") cbProgram.Text = "";
                if (cbYear.Text == "N/A") cbYear.Text = "";
            }
            else
            {
                cbProgram.Enabled = false;
                cbYear.Enabled = false;
                cbProgram.BackColor = Color.LightGray;
                cbYear.BackColor = Color.LightGray;
                cbProgram.Text = "N/A";
                cbYear.Text = "N/A";
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                if (_mode == FormMode.Register) PerformRegister();
                else PerformUpdate();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text))
            {
                XtraMessageBox.Show("Please fill in ID, Name, and Role.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);
            if (isStudent)
            {
                if (string.IsNullOrWhiteSpace(cbProgram.Text) || cbProgram.Text == "N/A" ||
                    string.IsNullOrWhiteSpace(cbYear.Text) || cbYear.Text == "N/A")
                {
                    XtraMessageBox.Show("Student requires a Program and Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private void PerformRegister()
        {
            _selectedUser.UserID = txtUserID.Text;
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Password = "123";

            if (_userRepo.AddUser(_selectedUser))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void PerformUpdate()
        {
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;

            if (_userRepo.EditUser(_selectedUser))
            {
                XtraMessageBox.Show("Account updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofd = new XtraOpenFileDialog())
            {
                ofd.Title = "Select Photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pePhoto.Image = Image.FromFile(ofd.FileName);
                        _selectedUser.UploadPath = ofd.FileName;
                    }
                    catch (Exception ex) { XtraMessageBox.Show("Error: " + ex.Message); }
                }
            }
        }
    }
}