using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;       
using SchoolClearanceSystem.Repository;

namespace SchoolClearanceSystem
{
    public enum FormMode { Register, Edit }

    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private FormMode _mode;
        private User _selectedUser;

        private UserRepository _userRepo = new UserRepository();

        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User();
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            SetupForm();
        }

        private void SetupForm()
        {
            //Populating data
            txtUserID.Text = _selectedUser.UserID;
            txtFullName.Text = _selectedUser.FullName;
            cbProgram.Text = _selectedUser.Program;
            cbYear.Text = _selectedUser.Year;
            cbRole.Text = _selectedUser.Role;
            txtDateCreated.Text = _mode == FormMode.Edit ? "Generated on " + DateTime.Now.ToShortDateString() : "Automatically Generated";

            if (!string.IsNullOrEmpty(_selectedUser.UploadPath))
            {
                try
                {
                    if (System.IO.File.Exists(_selectedUser.UploadPath))
                    {
                        pePhoto.Image = System.Drawing.Image.FromFile(_selectedUser.UploadPath);
                    }
                }
                catch (Exception)
                {
                    pePhoto.Image = null;
                }
            }
            //One form with two faces
            if (_mode == FormMode.Edit)
            {
                this.Text = "Edit Account Information";
                lblTitle.Text = "Edit Information";
                btnSave.Text = "Update Changes";
                txtUserID.ReadOnly = true;
                cbRole.Enabled = false;
            }
            else
            {
                this.Text = "Register New Account";
                lblTitle.Text = "Register Account";
                btnSave.Text = "Save Account";
                txtUserID.ReadOnly = false;
                cbRole.Enabled = true;
            }

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);

            if (!isStudent)
            {
                cbProgram.Enabled = false;
                cbYear.Enabled = false;
                cbProgram.BackColor = System.Drawing.Color.LightGray;
                cbYear.BackColor = System.Drawing.Color.LightGray;
                cbProgram.Text = "N/A";
                cbYear.Text = "N/A";
            }
            else
            {
                cbProgram.Enabled = true;
                cbYear.Enabled = true;
                cbProgram.BackColor = System.Drawing.Color.White;
                cbYear.BackColor = System.Drawing.Color.White;
            }
        }
        private void PerformRegister()
        {
            _selectedUser.UserID = txtUserID.Text;
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Password = "123";

            if (_userRepo.AddUser(_selectedUser))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void PerformUpdate()
        {
            _selectedUser.FullName = txtFullName.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Role = cbRole.Text;

            if (_userRepo.EditUser(_selectedUser))
            {
                XtraMessageBox.Show("Information successfully updated!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (_mode == FormMode.Register)
            {
                if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(cbProgram.Text) ||
                string.IsNullOrWhiteSpace(cbYear.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text))
                {
                    XtraMessageBox.Show("All fields must be filled for registration.", "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    XtraMessageBox.Show("Name cannot be empty.", "Validation Error");
                    return;
                }
            }

            if (_mode == FormMode.Register) PerformRegister();
            else PerformUpdate();
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            using (XtraOpenFileDialog ofdFilePicker = new XtraOpenFileDialog())
            {
                ofdFilePicker.Title = "Select Student Photo";
                ofdFilePicker.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofdFilePicker.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pePhoto.Image = System.Drawing.Image.FromFile(ofdFilePicker.FileName);

                        _selectedUser.UploadPath = ofdFilePicker.FileName;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("Could not load image: " + ex.Message, "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void pePhoto_EditValueChanged(object sender, EventArgs e)
        {
            var pictureEdit = sender as DevExpress.XtraEditors.PictureEdit;
            if (pictureEdit == null) return;

            if (pictureEdit.EditValue != null)
            {
                if (pictureEdit.EditValue is System.Drawing.Image img)
                {
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] photoBytes = ms.ToArray();
                    }
                }
                else if (pictureEdit.EditValue is byte[] rawBytes)
                {
                    byte[] photoBytes = rawBytes;
                }
            }
            else
            {
                byte[] photoBytes = null;
            }
        }
    
    }
}