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

     
        private string uploadedImagePath = string.Empty;

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

           
            btnViewPhoto.Enabled = false;
        }

        // ── Upload photo ─────────────────────────────────────────────────
        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofd = new XtraOpenFileDialog())
            {
                ofd.Title = "Select Student Photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    uploadedImagePath = ofd.FileName; 
                    btnViewPhoto.Enabled = true;     // Enable the view button!

                    XtraMessageBox.Show("Photo attached successfully! Click 'View Photo' to double check it.",
                        "Photo Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

      
       
        // ── Register button ──────────────────────────────────────────────
        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
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

          
            if (string.IsNullOrWhiteSpace(uploadedImagePath) || !File.Exists(uploadedImagePath))
            {
                XtraMessageBox.Show("Please select a valid photo file before registering.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User newUser = new User
            {
                UserID = txtUserID.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                Program = cmbProgram.Text,
                Year = cmbYear.Text,
                Role = "Student",
                Password = txtPassword.Text.Trim(),
                UploadPath = uploadedImagePath 
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
            txtFullName.Text = "";
            txtPassword.Text = "";
            uploadedImagePath = "";
            btnViewPhoto.Enabled = false;
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

        private void labelControl4_Click(object sender, EventArgs e)
        {

        }

        private void txtUserID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void btnViewPhoto_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uploadedImagePath) || !File.Exists(uploadedImagePath))
            {
                XtraMessageBox.Show("No valid photo file found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a temporary fluid popup form on the fly
            using (Form imagePopup = new Form())
            {
                PictureBox pb = new PictureBox();
                pb.Image = Image.FromFile(uploadedImagePath);
                pb.SizeMode = PictureBoxSizeMode.Zoom; // Maintains original photo aspect ratio
                pb.Dock = DockStyle.Fill;

                // Configure window styles
                imagePopup.Text = "Review Uploaded ID Photo";
                imagePopup.Size = new Size(500, 500); // Adjustable default size
                imagePopup.StartPosition = FormStartPosition.CenterScreen; // Centers perfectly on monitor
                imagePopup.FormBorderStyle = FormBorderStyle.SizableToolWindow; // Clean close window frame

                imagePopup.Controls.Add(pb);
                imagePopup.ShowDialog(); // Opens window as a modal block context
            }
        }
    }
}