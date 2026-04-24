using System;
using System.Windows.Forms;

namespace ClearanceSystem
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
            // This prevents the "silent fail" you experienced earlier
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                cmbProgram.SelectedItem == null ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields and select items from the dropdowns.",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. CREATE THE OBJECT
                User newUser = new User(
                    txtUserID.Text.Trim(),
                    txtFullName.Text.Trim(),
                    cmbProgram.SelectedItem.ToString(),
                    cmbYear.SelectedItem?.ToString() ?? "N/A", // Handles potential null year
                    cmbRole.SelectedItem.ToString(),
                    txtPassword.Text
                );

                // 3. ATTEMPT DATABASE SAVE
                DatabaseManager db = new DatabaseManager();
                db.SaveUser(newUser);

                // 4. SUCCESS FEEDBACK
                MessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the form for the next entry
                ClearFields();
            }
            catch (Exception ex)
            {
                // 5. ERROR FEEDBACK (In case of duplicate IDs or DB connection issues)
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtUserID.Text = "";
            txtFullName.Text = "";
            txtPassword.Text = "";
            cmbProgram.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            cmbRole.SelectedIndex = -1;
        }

        

        private void lblctrLogin_Click_1(object sender, EventArgs e)
        {
            // 1. Create the form
            Login loginForm = new Login();

            // 2. IMPORTANT: Tell the app to keep running even if this form closes
            loginForm.FormClosed += (s, args) => this.Close();

            // 3. Show Login
            loginForm.Show();

            // 4. Hide Registration instead of Closing it
            this.Hide();
        }
    }
    }
    

