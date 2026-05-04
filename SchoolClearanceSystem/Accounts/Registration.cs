using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

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
            // 1. Validation
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                cmbProgram.SelectedItem == null)
            {
                XtraMessageBox.Show("Please fill in all required fields.", "Validation Error");
                return;
            }

            // 2. Prepare User Object
            // 2. Prepare User Object
            User newUser = new User(
                txtUserID.Text.Trim(),
                txtFullName.Text.Trim(),
                cmbProgram.SelectedItem.ToString(),
                cmbYear.SelectedItem?.ToString() ?? "N/A",
                "Student",
                txtPassword.Text.Trim(), // Added .Trim() and a comma
                txtUploadPath.Text.Trim() // This is now the 7th parameter
            
            );

            // 3. Save and Verify
            // We check the result of the function. If it's false, the code inside {} is skipped.
            if (db.SaveUser(newUser))
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
            cmbProgram.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
        }

        private void lblctrLogin_Click_1(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            // This line creates the "dialog" manually so the error CS0103 goes away
            using (DevExpress.XtraEditors.XtraOpenFileDialog ofdFilePicker = new DevExpress.XtraEditors.XtraOpenFileDialog())
            {
                ofdFilePicker.Title = "Select Student Photo";
                ofdFilePicker.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (ofdFilePicker.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = ofdFilePicker.FileName;

                    // Assuming you have a TextBox to show the path to the user
                    txtUploadPath.Text = selectedPath;

                    XtraMessageBox.Show("File selected: " + selectedPath);
                }
            }
        }
    }
}
