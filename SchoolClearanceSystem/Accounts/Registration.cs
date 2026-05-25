using DevExpress.XtraEditors;
using System;
using System.Drawing;
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

          
            txtPassword.Properties.UseSystemPasswordChar = true;
            chkShowPassword.Properties.Caption = "Show Password";

            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };

            
            cmbProgram.Properties.Items.Clear();
            cmbProgram.Properties.Items.AddRange(new object[] { "BSIT" });
            cmbProgram.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

       
        // ── Register button ──────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                XtraMessageBox.Show("Fields cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbProgram.Text) || string.IsNullOrWhiteSpace(cmbYear.Text))
            {
                XtraMessageBox.Show("Please select a Program and Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User newUser = new User
            {
                UserID = txtUserID.Text.Trim(),
                FullName = txtLastName.Text.Trim(),
                Program = cmbProgram.Text,
                Year = cmbYear.Text,
                Role = "Student",
                Password = txtPassword.Text.Trim(),
              
            };

            if (_userRepo.AddUser(newUser))
            {
                XtraMessageBox.Show("Registration Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                XtraMessageBox.Show("User ID '" + newUser.UserID + "' is already taken. Choose another.",
                    "Duplicate User ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserID.Focus();
            }
        }

        // ── Clear all fields ─────────────────────────────────────────────
        private void ClearFields()
        {
            txtUserID.Text = "";
            txtLastName.Text = "";
            txtPassword.Text = "";
            cmbProgram.EditValue = null;
            cmbYear.EditValue = null;
        }

        private void lblctrLogin_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
            this.Hide();
        }

        private void labelControl8_Click(object sender, EventArgs e)
        {

        }

        private void labelControl7_Click(object sender, EventArgs e)
        {

        }
    }
}