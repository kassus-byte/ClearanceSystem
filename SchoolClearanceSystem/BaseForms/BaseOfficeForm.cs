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

        private string currentStatusFilter = "All";
        public string OfficeName { get; set; } = "Unknown Office";

        public BaseOfficeForm()
        {
            InitializeComponent();

            // Wire form lifecycle initializations securely
            this.Load += BaseOfficeForm_Load;
            btnAllFilter.Click += (s, e) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, e) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, e) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, e) => SetStatusFilter("On Hold");

            // Wire text change queries
            txtSearch.TextChanged += TxtSearch_TextChanged;
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


        #endregion

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show(
                "Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                Login login = new Login();
                login.Show();
                this.Hide();
                this.Close();
            }
        }

        private void ApplyUnifiedFilter()
        {
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            string filterCriteria = string.Empty;

            // 1. Evaluate Row Status Criteria
            if (currentStatusFilter != "All")
            {
                filterCriteria = $"[Status] = '{currentStatusFilter}'";
            }

            // 2. Evaluate Search Wildcard Values across structural layout indices
            string searchText = txtSearch.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrEmpty(searchText))
            {
                string searchCriteria = $"([UserID] LIKE '%{searchText}%' OR [FullName] LIKE '%{searchText}%' OR [Program] LIKE '%{searchText}%')";

                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = searchCriteria;
                else
                    filterCriteria += $" AND {searchCriteria}";
            }

            // 3. Post Filter Strings directly into the active layout engine
            view.ActiveFilterString = filterCriteria;
        }

            private void SetStatusFilter(string status)
        {
            currentStatusFilter = status;
            ApplyUnifiedFilter();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyUnifiedFilter();
        }


        private void btnAction_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            dynamic selectedRequest = view.GetRow(view.FocusedRowHandle);
            if (selectedRequest == null) return;

            string studentId = selectedRequest.UserID?.ToString();
            string targetOffice = this.OfficeName;
            string targetStatus = string.Empty;

            string buttonTag = e.Button.Tag?.ToString();
            switch (buttonTag)
            {
                case "btnApprove": targetStatus = "Approved"; break;
                case "btnOnHold": targetStatus = "On Hold"; break;
                default: return;
            }

            ClearanceRepository repo = new ClearanceRepository();
            string defaultRemarks = $"Processed by {targetOffice} Office";

            bool isSuccess = repo.UpdateRequestStatus(studentId, targetOffice, targetStatus, defaultRemarks);

            if (isSuccess)
            {
                XtraMessageBox.Show($"Clearance status updated to '{targetStatus}' successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // OOP REFACTOR: Keep tracking layout consistent by updating memory properties directly!
                // This updates BOTH status strings and remarks cells instantly without hiding the active line row.
                selectedRequest.Status = targetStatus;
                selectedRequest.Remarks = defaultRemarks;

                view.RefreshRow(view.FocusedRowHandle);
            }
            else
            {
                XtraMessageBox.Show("Database update execution rejected. Check connection states.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenTargetFile(string targetPath)
        {
            if (string.IsNullOrEmpty(targetPath) || !System.IO.File.Exists(targetPath))
            {
                XtraMessageBox.Show("No file uploaded yet, or the file no longer exists.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(targetPath)
                {
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not open the file: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProof_Click(object sender, EventArgs e)
        {
            // 1. Safe Interface Cast: Extract the current active DevExpress GridView view context
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            // 2. Focused Row Access: Capture the dynamic backend data model for the highlighted row
            dynamic selectedRequest = view.GetRow(view.FocusedRowHandle);
            if (selectedRequest == null) return;

            try
            {
                // 3. Dynamic Property Extraction: Read the string holding the raw file path
                // Note: If your database table field or query alias uses something like "ProofFilePath", change "Proof" to match it!
                string proofPath = selectedRequest.Proof?.ToString();

                // 4. Encapsulation / Delegation: Route the file location to your existing OS execution helper
                OpenTargetFile(proofPath);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"File System Sync Error: Unable to extract file tracking structure. {ex.Message}",
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
