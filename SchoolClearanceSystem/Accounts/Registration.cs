using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using SchoolClearanceSystem.Models;      // For User model
using SchoolClearanceSystem.Repository;  // For UserRepository

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        // REFACTORED: Use the UserRepository instead of DatabaseManager
        private readonly UserRepository _userRepo = new UserRepository();

        public Registration()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            // 1. Basic Validation
            if (string.IsNullOrWhiteSpace(txtUserID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                XtraMessageBox.Show("Fields cannot be empty.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. File Validation
            string path = txtUploadPath.Text.Trim();
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                XtraMessageBox.Show("Please select a valid photo file before registering.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Create the User object (OOP: Encapsulation)
            User newUser = new User
            {
                UserID = txtUserID.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Program = cmbProgram.SelectedItem?.ToString() ?? "N/A",
                Year = cmbYear.SelectedItem?.ToString() ?? "N/A",
                Role = "Student",
                Password = txtPassword.Text.Trim(),
                UploadPath = path
            };

            // 4. Save to Database using the Repository
            // We call _userRepo.SaveUser instead of db.SaveUser
            if (_userRepo.SaveUser(newUser))
            {
                XtraMessageBox.Show("Registration Successful!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
        }

        private void ClearFields()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            txtUploadPath.Text = "";
            cmbProgram.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofdFilePicker = new XtraOpenFileDialog())
            {
                ofdFilePicker.Title = "Select Student Photo";
                ofdFilePicker.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (ofdFilePicker.ShowDialog() == DialogResult.OK)
                {
                    txtUploadPath.Text = ofdFilePicker.FileName;
                }
            }
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