using System;
using System.Windows.Forms;
using DevExpress.XtraEditors; // Added to use XtraMessageBox for a consistent UI

namespace SchoolClearanceSystem
{
    public partial class Registration : DevExpress.XtraEditors.XtraForm
    {
        public Registration()
        {
            SQLitePCL.Batteries.Init();
            InitializeComponent();
        }

        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            // 1. DEFENSIVE VALIDATION 
            // Removed cmbRole check since students no longer select their role
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbProgram.SelectedItem == null)
            {
                XtraMessageBox.Show("Please fill in all required fields and select your program.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. CREATE THE OBJECT
                // We hardcode "Student" as the 5th parameter (Role)
                User newUser = new User(
                    txtUserID.Text.Trim(),
                    txtFullName.Text.Trim(),
                    cmbProgram.SelectedItem.ToString(),
                    cmbYear.SelectedItem?.ToString() ?? "N/A",
                    "Student", // <--- ROLE IS HARDCODED HERE
                    txtPassword.Text
                );

                // 3. ATTEMPT DATABASE SAVE
                DatabaseManager db = new DatabaseManager();
                db.SaveUser(newUser);

                // 4. SUCCESS FEEDBACK
                XtraMessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the form for the next entry
                ClearFields();
            }
            catch (Exception ex)
            {
                // 5. ERROR FEEDBACK
                XtraMessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            cmbProgram.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            // cmbRole is no longer here to clear
        }

        private void lblctrLogin_Click_1(object sender, EventArgs e)
        {
            Login loginForm = new Login();

            // This ensures the application closes properly when navigating back and forth
            loginForm.FormClosed += (s, args) => this.Close();

            loginForm.Show();
            this.Hide();
        }
    }
}