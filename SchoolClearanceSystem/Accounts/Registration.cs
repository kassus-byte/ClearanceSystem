// Registration.cs
using DevExpress.XtraEditors;
using System;
using System.IO;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        private readonly UserRepository _userRepo = new UserRepository();

        public Registration()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();

           

            // ── Populate DevExpress ComboBoxEdit for Program ───────────────
            cmbProgram.Properties.Items.Clear();
            cmbProgram.Properties.Items.AddRange(new object[]
            {
                "BSIT"
            });
            cmbProgram.Properties.TextEditStyle =
                DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            // ── Populate DevExpress ComboBoxEdit for Year ─────────────────
            cmbYear.Properties.Items.Clear();
            cmbYear.Properties.Items.AddRange(new object[]
            {
                "1st Year", "2nd Year", "3rd Year", "4th Year"
            });
            cmbYear.Properties.TextEditStyle =
                DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        // ── Register button ──────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            // Validate text fields
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                XtraMessageBox.Show("Fields cannot be empty.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate ComboBoxEdit — use .Text for DevExpress ComboBoxEdit
            if (string.IsNullOrWhiteSpace(cmbProgram.Text) ||
                string.IsNullOrWhiteSpace(cmbYear.Text))
            {
                XtraMessageBox.Show("Please select a Program and Year.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate photo
            string path = txtUploadPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                XtraMessageBox.Show("Please select a valid photo file before registering.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Build User — admin dashboard receives via GetAllUsers()
            User newUser = new User
            {
                UserID = txtUserID.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Program = cmbProgram.Text,
                Year = cmbYear.Text,
                Role = "Student",
                Password = txtPassword.Text.Trim(),
                UploadPath = path
            };

            if (_userRepo.AddUser(newUser))
            {
                XtraMessageBox.Show("Registration Successful!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                XtraMessageBox.Show("User ID '" + newUser.UserID + "' is already taken. Choose another.",
                    "Duplicate User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
            }
        }

        // ── Upload photo ─────────────────────────────────────────────────
        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofd = new XtraOpenFileDialog())
            {
                ofd.Title = "Select Student Photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                    txtUploadPath.Text = ofd.FileName;
            }
        }

        // ── Clear all fields ─────────────────────────────────────────────
        private void ClearFields()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            txtUploadPath.Text = "";
            cmbProgram.EditValue = null;
            cmbYear.EditValue = null;
        }

        // ── Go back to Login ─────────────────────────────────────────────
        private void lblctrLogin_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }
    }
}