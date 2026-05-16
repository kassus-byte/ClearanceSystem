using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository; // CONNECTS TO DATABASE VIA: UserRepository -> BaseRepository -> DatabaseManager
using System.Drawing;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: ENUMERATION STATE MANIPULATION
    /// FormMode functions as a strongly-typed collection of states. Instead of passing brittle 
    /// tracking options like string flags ("Register", "Edit"), Enums guarantee compilation-safe 
    /// behavior switches across the form lifecycle.
    /// </summary>
    public enum FormMode { Register, Edit }

    /// <summary>
    /// OOP CONCEPT: POLYMORPHISM (State-Driven Form Layouts)
    /// This single form acts as a dual-purpose layout canvas. By evaluating the initialized operational 
    /// '_mode' state variable, it morphs dynamically into either a Registration form or an Modification viewer.
    /// </summary>
    public partial class UserInfoForm : DevExpress.XtraEditors.XtraForm
    {
        // Encapsulated Class Attributes (Data Fields) protecting data visibility inside the execution scope
        private FormMode _mode;
        private User _selectedUser;
        private UserRepository _userRepo = new UserRepository();

        /// <summary>
        /// OOP CONCEPT: PARAMETERIZED CONSTRUCTOR & DEFAULT OBJECT FALLBACKS
        /// The constructor forces caller forms (like AdminDashboard) to explicitly declare its operational state.
        /// 
        /// HOW IT CONNECTS:
        /// 1. AdminDashboard calls: new UserInfoForm(FormMode.Register, null) or new UserInfoForm(FormMode.Edit, selectedUser)
        /// 2. The 'user = null' parameter showcases optional parameter defaults. If null, the fallback statement 
        ///    instantiates a fresh, empty clean 'User' model memory container automatically.
        /// </summary>
        public UserInfoForm(FormMode mode, User user = null)
        {
            InitializeComponent();
            _mode = mode;
            _selectedUser = user ?? new User(); // Null-coalescing operator assigns fallback object allocation

            // Event-Driven Programming: Attaches a custom callback listener delegate to the DevExpress ComboBox control pipeline
            cbRole.SelectedIndexChanged += cbRole_SelectedIndexChanged;
        }

        private void UserInfoForm_Load(object sender, EventArgs e)
        {
            SetupForm(); // Coordinates visual properties layout based on state bindings
        }

        /// <summary>
        /// HOW IT WORKS - DATA RECOVERY & STATE RENDERING:
        /// This method transfers attributes locked within our pure C# data entity class object ('_selectedUser') 
        /// directly into visual UI fields on screen, fulfilling standard Model-View separation bindings.
        /// </summary>
        private void SetupForm()
        {
            // Bind internal object properties directly out to user-facing input text controls
            txtUserID.Text = _selectedUser.UserID;
            txtFullName.Text = _selectedUser.FullName;
            cbProgram.Text = _selectedUser.Program;
            cbYear.Text = _selectedUser.Year;
            cbRole.Text = _selectedUser.Role;
            txtDateCreated.Text = _mode == FormMode.Edit ? "Generated on " + DateTime.Now.ToShortDateString() : "Automatically Generated";

            // Safe File I/O Management tracking structural image assets
            if (!string.IsNullOrEmpty(_selectedUser.UploadPath))
            {
                try
                {
                    // Validation abstraction layer verifying if path coordinates actively pinpoint a clean OS file entry
                    if (System.IO.File.Exists(_selectedUser.UploadPath))
                    {
                        // Streams absolute file content directly out into DevExpress PictureEdit canvas containers
                        pePhoto.Image = Image.FromFile(_selectedUser.UploadPath);
                    }
                }
                catch (Exception) { pePhoto.Image = null; } // Catches exceptions to protect program lifecycle safety
            }

            // Polymorphic Behavior Adjustment Block
            if (_mode == FormMode.Edit)
            {
                this.Text = "Edit Account Information";
                lblTitle.Text = "Edit Information";
                btnSave.Text = "Update Changes";
                txtUserID.ReadOnly = true; // Business Rule: System Keys cannot be updated once saved
                cbRole.Enabled = false;   // Business Rule: Roles cannot be flipped in editing mode
            }
            else
            {
                this.Text = "Register New Account";
                lblTitle.Text = "Register Account";
                btnSave.Text = "Save Account";
                txtUserID.ReadOnly = false;
                cbRole.Enabled = true;
            }

            ToggleFieldsBasedOnRole(); // Runs evaluation checking context configuration upon form entry
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleFieldsBasedOnRole(); // Recalculates screen state properties whenever user alters index values
        }

        /// <summary>
        /// OOP CONCEPT: BUSINESS RULE ENFORCEMENT & INTERFACE ADAPTATION
        /// Encapsulates application processing rules. Since administrative role tiers (Treasurer, SSG) 
        /// do not possess course sections or program paths, the UI blocks structural fields conditionally.
        /// </summary>
        private void ToggleFieldsBasedOnRole()
        {
            if (string.IsNullOrWhiteSpace(cbRole.Text)) return;

            // Conditional state checking forcing system behavioral changes at runtime
            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);

            if (isStudent)
            {
                cbProgram.Enabled = true;
                cbYear.Enabled = true;
                cbProgram.BackColor = Color.White;
                cbYear.BackColor = Color.White;

                if (cbProgram.Text == "N/A") cbProgram.Text = "";
                if (cbYear.Text == "N/A") cbYear.Text = "";
            }
            else
            {
                // Disables controls dynamically to block dirty data formatting entry errors
                cbProgram.Enabled = false;
                cbYear.Enabled = false;
                cbProgram.BackColor = Color.LightGray;
                cbYear.BackColor = Color.LightGray;
                cbProgram.Text = "N/A";
                cbYear.Text = "N/A";
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Routes execution logic streams dynamically based on Enum configuration flags
                if (_mode == FormMode.Register) PerformRegister();
                else PerformUpdate();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) ||
                string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text))
            {
                XtraMessageBox.Show("Please fill in ID, Name, and Role.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            bool isStudent = cbRole.Text.Equals("Student", StringComparison.OrdinalIgnoreCase);
            if (isStudent)
            {
                if (string.IsNullOrWhiteSpace(cbProgram.Text) || cbProgram.Text == "N/A" ||
                    string.IsNullOrWhiteSpace(cbYear.Text) || cbYear.Text == "N/A")
                {
                    XtraMessageBox.Show("Student requires a Program and Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// HOW IT CONNECTS TO DATABASE MANAGER (Write Pipeline):
        /// 1. Hydrates the tracking fields of our data model object variable directly from text elements.
        /// 2. Hands over the package object structure to '_userRepo.AddUser(_selectedUser)'.
        /// 3. Repository calls 'DatabaseManager.GetConnection()' to gain stream access into SQLite.
        /// 4. Dapper automatically decomposes the 'User' class model properties and inserts them safely.
        /// </summary>
        private void PerformRegister()
        {
            _selectedUser.UserID = txtUserID.Text.Trim();
            _selectedUser.FullName = txtFullName.Text.Trim();
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;
            _selectedUser.Password = "123"; // Business Rule Note: Default password assigned automatically for demo reset tracking

            if (_userRepo.AddUser(_selectedUser))
            {
                this.DialogResult = DialogResult.OK; // Sets communication flag so calling Admin Form can perform automated refreshing
                this.Close();
            }
        }

        /// <summary>
        /// HOW IT CONNECTS TO DATABASE MANAGER (Update Pipeline):
        /// Transfers user visual updates back inside properties of the tracking reference model entity, 
        /// then signals an execution query string down through data access repositories.
        /// </summary>
        private void PerformUpdate()
        {
            _selectedUser.FullName = txtFullName.Text.Trim();
            _selectedUser.Role = cbRole.Text;
            _selectedUser.Program = cbProgram.Text;
            _selectedUser.Year = cbYear.Text;

            if (_userRepo.EditUser(_selectedUser))
            {
                XtraMessageBox.Show("Account updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            // OOP CONCEPT: ABSTRACTION VIA DIALOG COMPONENTS
            // The XtraOpenFileDialog wraps the native operating system file explorer interfaces.
            using (XtraOpenFileDialog ofd = new XtraOpenFileDialog())
            {
                ofd.Title = "Select Photo";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pePhoto.Image = Image.FromFile(ofd.FileName);

                        // ENCAPSULATION BENEFIT: We only store the directory location path string 
                        // pointer variable inside the user model profile data record.
                        _selectedUser.UploadPath = ofd.FileName;
                    }
                    catch (Exception ex) { XtraMessageBox.Show("Error: " + ex.Message); }
                }
            }
        }
    }
}