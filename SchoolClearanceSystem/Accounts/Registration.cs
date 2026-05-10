using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        DatabaseManager db = new DatabaseManager();

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
                XtraMessageBox.Show("Fields cannot be empty.");
                return;
            }

            // 2. File Validation - Define the 'path' variable here
            string path = txtUploadPath.Text.Trim();

            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
            {
                XtraMessageBox.Show("Please select a valid photo file before registering.", "Validation Error");
                return;
            }

            // 3. Prepare the automatic date
            string dateStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 4. Create the User object
            // Ensure the number of items matches your User constructor:
            // (id, name, prog, year, role, pass, path, date)
            User newUser = new User(
                txtUserID.Text.Trim(),
                txtFullName.Text.Trim(),
                cmbProgram.SelectedItem?.ToString() ?? "N/A",
                cmbYear.SelectedItem?.ToString() ?? "N/A",
                "Student",
                txtPassword.Text.Trim(),
                path,        // This variable is now recognized
                dateStamp    // This handles the automatic @date
            );

            // 5. Save to Database
            if (db.SaveUser(newUser))
            {
                XtraMessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
        }
        private void ClearFields()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            txtUploadPath.Text = ""; // Added this to clear path too
            cmbProgram.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (DevExpress.XtraEditors.XtraOpenFileDialog ofdFilePicker = new DevExpress.XtraEditors.XtraOpenFileDialog())
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