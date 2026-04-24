using ClearanceSystem.Dashboards;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClearanceSystem
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

            // 1. Basic check so we don't even talk to the DB if fields are empty
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Please enter both ID and Password.");
                return;
            }

            // 2. Check the Database
            DatabaseManager db = new DatabaseManager();
            bool isValid = db.ValidateLogin(id, pass);

            if (isValid)
            {
                // SUCCESS: Open the Dashboard
                StudentDashboard studentDashboard = new StudentDashboard();
                studentDashboard.FormClosed += (s, args) => this.Close();
                studentDashboard.Show();
                this.Hide();
            }
            else
            {
                // FAIL: Keep the user here and warn them
                MessageBox.Show("Invalid UserID or Password. Please try again.",
                                "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = ""; // Clear password for security
            }
        }
    }
}