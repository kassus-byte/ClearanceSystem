using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: POLYMORPHISM & FORM INHERITANCE (Base Blueprint Architecture)
    /// This abstract base controller handles corporate layout styling, session identity parsing, 
    /// and dynamic data-binding workflows for all department desks (SSG, Treasurer, Tech Office).
    /// </summary>
    public partial class BaseOfficeForm : XtraForm
    {
        // OOP CONCEPT: ENCAPSULATION
        // Mapped runtime state container used as the primary lookup parameter for data filtering.
        // Inheriting child forms assign their department code to this property inside their constructors.
        public string OfficeName { get; set; } = "Unknown Office";

        public BaseOfficeForm()
        {
            InitializeComponent();

            // Wire form lifecycle initializations securely
            this.Load += BaseOfficeForm_Load;
        }

        private void BaseOfficeForm_Load(object sender, EventArgs e)
        {
            SetupIdentity();
            LoadPendingClearanceRequests(); // Populate grid view data immediately on initialization
        }

        /// <summary>
        /// Reads operational session tokens to configure contextual branding labels at runtime.
        /// </summary>
        private void SetupIdentity()
        {
            if (Session.CurrentUser != null)
            {
                lblFullName.Text = Session.CurrentUser.FullName;
                lblRole.Text = Session.CurrentUser.Role;

                // Dynamic UI window caption mutation
                this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
            }
        }

        #region Navigation Control Flow Routine Managers

        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadPendingClearanceRequests(); // Refresh the table tracking view when returning home
        }

        private void sbOfficeClearanceRequest_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeClearanceRequest;
        }

        private void sbOfficeRequirements_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeRequirements;
        }

        private void sbOfficeReports_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeReports;
        }

        #endregion

        #region Database Processing and Presentation Binding Pipeline

        /// <summary>
        /// HOW IT WORKS (Data Hydration Engine):
        /// Pulls collections from ClearanceRepository filtered by the active office context,
        /// then binds the memory structures directly into the DevExpress GridControl layout engine.
        /// </summary>
        protected void LoadPendingClearanceRequests()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

                // Fetch data items matching our workspace identity parameter context
                var pendingDataList = repo.GetRequestsForOffice(this.OfficeName);

                // Assign data items directly to your layout table grid container
                gcBaseOfficeForm.DataSource = pendingDataList;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not bind office requests table rows: {ex.Message}",
                    "Data Retrieval Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Session De-Authentication Logic

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show(
                "Are you sure you want to log out of the system?",
                "Confirm Sign Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Session.CurrentUser = null;

                Login login = new Login();
                login.Show();

                this.Hide();
                this.Close();
            }
        }

        #endregion
    }
}