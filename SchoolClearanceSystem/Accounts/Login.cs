using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;       // To recognize User class
using SchoolClearanceSystem.Repository;   // To use UserRepository

namespace SchoolClearanceSystem
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        // OOP: Encapsulation - The Login form uses the Repository to handle data
        private readonly UserRepository _userRepo = new UserRepository();

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string id = txtUserID.Text.Trim();
            string pass = txtPassword.Text;

            // 1. Validation
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                XtraMessageBox.Show("Please enter both ID and Password.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Authenticate using the Repository
            bool isValid = _userRepo.ValidateLogin(id, pass);

            if (isValid)
            {
                // 3. Save User to Session (Short-term memory)
                Session.CurrentUser = _userRepo.GetUserDetails(id);

                if (Session.CurrentUser != null)
                {
                    Form nextForm = null;

                    // 4. Redirection logic based on role
                    switch (Session.CurrentUser.Role)
                    {
                        case "Admin":
                            nextForm = new SchoolClearanceSystem.Dashboard.AdminDashboard();
                            break;

                        case "Student":
                            // nextForm = new StudentPortal(); // Uncomment when ready
                            break;

                        case "Treasurer":
                        case "Library":
                        case "Registrar":
                            // You can create a generic OfficeDashboard or specific ones
                            nextForm = new SchoolClearanceSystem.Dashboard.AdminDashboard();
                            break;

                        default:
                            XtraMessageBox.Show("Your role is not recognized. Contact Admin.", "Access Denied");
                            return;
                    }

                    if (nextForm != null)
                    {
                        // Clean up: Close login when the dashboard is closed
                        nextForm.FormClosed += (s, args) => this.Close();
                        nextForm.Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Invalid UserID or Password.", "Login Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}