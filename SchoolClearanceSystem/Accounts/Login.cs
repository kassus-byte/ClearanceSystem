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

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                XtraMessageBox.Show("Please enter both ID and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseManager db = new DatabaseManager();
            bool isValid = db.ValidateLogin(id, pass);

            if (isValid)
            {
                Session.CurrentUser = db.GetUserDetails(id);

                if (Session.CurrentUser != null)
                {
                    Form nextForm = null;

                    // REDIRECTION LOGIC BASED ON ROLE
                    switch (Session.CurrentUser.Role)
                    {
                        case "Student":
                            nextForm = new StudentPortal();
                            break;

                        case "Treasurer":
                            // Ensure you have added: using SchoolClearanceSystem.Dashboard;
                            nextForm = new SchoolClearanceSystem.Dashboard.TreasurerDashboard();
                            break;

                        default:
                            XtraMessageBox.Show("Your role is not recognized. Contact Admin.", "Access Denied");
                            return;
                    }

                    // Standardize form closing and showing
                    nextForm.FormClosed += (s, args) => this.Close();
                    nextForm.Show();
                    this.Hide();
                }
            }
            else
            {
                XtraMessageBox.Show("Invalid UserID or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    }
