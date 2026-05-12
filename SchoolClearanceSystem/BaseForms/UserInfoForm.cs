using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace SchoolClearanceSystem
{
    // Mode switcher for the form
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private FormMode _mode;
        private User _selectedUser; // OOP: Use the User class, not 'dynamic'
        private DatabaseManager db = new DatabaseManager();

        // Constructor: Now accepts a strongly-typed User object
        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;

            // If editing, use the passed user. If registering, start with a fresh object.
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
                // --- EDIT MODE SETUP ---
                this.Text = "Edit Account Information";
                btnSave.Text = "Update Changes";

                // Load data from the User object into the TextBoxes
                txtUserID.Text = _selectedUser.UserID;
                txtUserID.ReadOnly = true; // Cannot change ID during edit
                txtFullName.Text = _selectedUser.FullName;
                cbProgram.Text = _selectedUser.Program;
                cbYear.Text = _selectedUser.Year;
                cbRole.Text = _selectedUser.Role;

                // Show the creation date
                txtDateCreated.Text = _selectedUser.DateCreated ?? "N/A";
                txtDateCreated.ReadOnly = true;
                txtDateCreated.Visible = true;
            }
            else
            {
                // --- REGISTER MODE SETUP ---
                this.Text = "Register New Account";
                btnSave.Text = "Save Account";

                txtUserID.ReadOnly = false;
                txtUserID.Text = "";
                txtFullName.Text = "";
                txtDateCreated.Text = "Automatically Generated";
            }
        }

        private void PerformRegister()
        {
            // Map the UI values to the User object properties
            _selectedUser.UserID = txtUserID.Text;
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Password = "123"; // Default initial password

            // DatabaseManager handles the DateCreated inside SaveUser
            if (db.SaveUser(_selectedUser))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void PerformUpdate()
        {
            // 1. Update the object with the current text from the boxes
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Role = cbRole.Text;

            // 2. Pass the WHOLE object to the database
            if (db.UpdateUser(_selectedUser))
            {
                XtraMessageBox.Show("Information successfully updated!", "Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                XtraMessageBox.Show("Please enter a full name.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUserID.Text))
            {
                XtraMessageBox.Show("Please enter a User ID.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Decide which database action to take
            if (_mode == FormMode.Register)
            {
                PerformRegister();
            }
            else
            {
                PerformUpdate();
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}