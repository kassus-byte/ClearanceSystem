using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System.Drawing;
using System.IO;

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : XtraForm
    {
        private FormMode _mode;
        private User _selectedUser;
        private UserRepository _userRepo = new UserRepository();

        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User();
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
        }

        private void UserInfoForm_Load(object sender, EventArgs e) => SetupForm();

        private void SetupForm()
        {
            txtUserID.Text = _selectedUser.UserID;
            txtFullName.Text = _selectedUser.FullName;
            cbProgram.Text = _selectedUser.Program;
            cbYear.Text = _selectedUser.Year;
            cbRole.Text = _selectedUser.Role;
            txtDateCreated.Text = _mode == FormMode.Edit ? "Generated on " + DateTime.Now.ToShortDateString() : "Automatically Generated";

            // Abstracted Image Stream Loader
            if (!string.IsNullOrEmpty(_selectedUser.UploadPath) && File.Exists(_selectedUser.UploadPath))
            {
                try { pePhoto.Image = Image.FromFile(_selectedUser.UploadPath); }
                catch { pePhoto.Image = null; }
            }

            // OOP CONCEPT: MAPPING & DATA DRIVEN PROPERTY CONFIGURATION
            // Condenses extensive structural if/else UI assignments into declarative mappings
            bool isEdit = (_mode == FormMode.Edit);
            this.Text = isEdit ? "Edit Account Information" : "Register New Account";
            lblTitle.Text = isEdit ? "Edit Information" : "Register Account";
            btnSave.Text = isEdit ? "Update Changes" : "Save Account";
            txtUserID.ReadOnly = isEdit;
            cbRole.Enabled = !isEdit;

            ToggleFieldsBasedOnRole();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e) => ToggleFieldsBasedOnRole();

        private void ToggleFieldsBasedOnRole()
        {
            if (string.IsNullOrWhiteSpace(cbRole.Text)) return;

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);

            // OOP CONCEPT: ENCAPSULATION & UNIFIED FIELD MUTATORS
            cbProgram.Enabled = isStudent;
            cbYear.Enabled = isStudent;
            cbProgram.BackColor = isStudent ? Color.White : Color.LightGray;
            cbYear.BackColor = isStudent ? Color.White : Color.LightGray;

            if (isStudent)
            {
                if (cbProgram.Text == "N/A") cbProgram.Text = "";
                if (cbYear.Text == "N/A") cbYear.Text = "";
            }
            else
            {
                cbProgram.Text = "N/A";
                cbYear.Text = "N/A";
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            
            if (_mode == FormMode.Register) PerformRegister();
            else PerformUpdate();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(cbRole.Text))
            {
                XtraMessageBox.Show("Please fill in ID, Name, and Role.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(cbProgram.Text) || cbProgram.Text == "N/A" || string.IsNullOrWhiteSpace(cbYear.Text) || cbYear.Text == "N/A")
                {
                    XtraMessageBox.Show("Student requires a Program and Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        // OOP CONCEPT: STATE SYNC DATA CAPTURE
        private void CaptureFormState()
        {
            _selectedUser.FullName = txtFullName.Text.Trim();
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
        }

        private void PerformRegister()
        {
            _selectedUser.UserID = txtUserID.Text.Trim();
            _selectedUser.Password = "123";
            CaptureFormState();

            if (_userRepo.AddUser(_selectedUser)) CloseFormWithResult(DialogResult.OK);
        }

        private void PerformUpdate()
        {
            CaptureFormState();

            if (_userRepo.EditUser(_selectedUser))
            {
                XtraMessageBox.Show("Account updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CloseFormWithResult(DialogResult.OK);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e) => CloseFormWithResult(DialogResult.Cancel);

        private void CloseFormWithResult(DialogResult result)
        {
            this.DialogResult = result;
            this.Close();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofd = new XtraOpenFileDialog())
            {
                ofd.Title = "Select Photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    pePhoto.Image = Image.FromFile(ofd.FileName);
                    _selectedUser.UploadPath = ofd.FileName;
                }
                catch (Exception ex) 
                { 
                    XtraMessageBox.Show("Error: " + ex.Message); 
                }
            }
        }
    }
}