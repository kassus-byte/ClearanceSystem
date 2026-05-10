using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private FormMode _mode;
        private dynamic _selectedData;
        DatabaseManager db = new DatabaseManager();

        // Constructor
        public UserInfoForm(FormMode mode, dynamic data = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedData = data;
           
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            SetupForm();
        }

        private void SetupForm()
        {
            if (_mode == FormMode.Edit && _selectedData != null)
            {
                // --- EDIT MODE ONLY ---
                this.Text = "Edit Student Information";
                btnSave.Text = "Update Changes";

                txtUserID.Text = _selectedData["UserID"].ToString();
                txtUserID.ReadOnly = true;
                txtFullName.Text = _selectedData["FullName"].ToString();
                cbProgram.Text = _selectedData["Program"].ToString();
                cbYear.Text = _selectedData["Year"].ToString();

                // Display the Date (Hidden or Read-Only)
                txtDateCreated.Text = _selectedData["DateCreated"]?.ToString() ?? "N/A";
                txtDateCreated.ReadOnly = true;
                txtDateCreated.Visible = true; // Show it so admin can see when it was made
            }
            else
            {
                // --- REGISTER MODE ONLY ---
                this.Text = "Register New Account";
                btnSave.Text = "Save Account";

                txtUserID.ReadOnly = false;
                txtUserID.Text = "";
                txtFullName.Text = "";

                // Hide the Date field or set to "Auto"
                txtDateCreated.Text = "Automatically Generated";
                txtDateCreated.ReadOnly = true;
                // Optional: txtDateCreated.Visible = false; 
            }
        }
        private void PerformRegister()
        {
            User newUser = new User
            {
                UserID = txtUserID.Text,
                FullName = txtFullName.Text,
                // USE .Text TO GET THE ACTUAL SELECTED STRING (e.g., "Treasurer")
                Role = cbRolee.Text,
                Program = cbProgram.Text,
                Year = cbYear.Text,
                Password = "123",
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            if (db.SaveUser(newUser))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void PerformUpdate()
        {
            // Use the new UpdateUser method we added to DatabaseManager
            bool success = db.UpdateUser(
                txtUserID.Text,
                txtFullName.Text,
                cbProgram.Text,
                cbYear.Text
            );

            if (success)
            {
                XtraMessageBox.Show("Information successfully updated!", "Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // 1. Perform common validation here (e.g., check if name is empty)
            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                XtraMessageBox.Show("Please enter a name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Branch logic based on the mode
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