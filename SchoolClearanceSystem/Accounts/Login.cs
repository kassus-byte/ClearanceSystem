using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtUserID.Text.Trim();
            string pass = txtPassword.Text;

            // 1. Basic check
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                XtraMessageBox.Show("Please enter both ID and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Check the Database
            DatabaseManager db = new DatabaseManager();
            bool isValid = db.ValidateLogin(id, pass);

            if (isValid)
            {
                // SUCCESS: 
                // A. Fetch full user details and save them to our global Session
                Session.CurrentUser = db.GetUserDetails(id);

                // B. Verify we actually got data back before proceeding
                if (Session.CurrentUser != null)
                {
                    // C. Open the Student Portal
                    StudentPortal studentPortal = new StudentPortal();

                    // This ensures that when the Portal is closed, the hidden Login form also closes (cleaning up memory)
                    studentPortal.FormClosed += (s, args) => this.Close();

                    studentPortal.Show();
                    this.Hide();
                }
                else
                {
                    XtraMessageBox.Show("User details could not be loaded. Please contact admin.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // FAIL: Warn the user
                XtraMessageBox.Show("Invalid UserID or Password. Please try again.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtPassword.Focus();
            }
        }
    }
}