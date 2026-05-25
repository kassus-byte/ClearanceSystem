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

            // Wire text change queries immediately upon constructor registration
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        /// <summary>
        /// LIFECYCLE SAFE REFACTOR: Replacing the 'this.Load' event subscription with a native 
        /// OnLoad override. This prevents race conditions where the database executes before 
        /// the child forms finish injecting their initialization strings.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode) { base.OnLoad(e); return; }

            btnAllFilter.Click += (s, ev) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, ev) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, ev) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, ev) => SetStatusFilter("On Hold");

            SetupIdentity();
            LoadPendingClearanceRequests();
            LoadDashboardStats(); // ← ADD THIS
            base.OnLoad(e);
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
            LoadPendingClearanceRequests();
            LoadDashboardStats(); // ← ADD THIS
        }

        private void sbOfficeClearanceRequest_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeClearanceRequest;
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
            // STEP 1: Get the active GridView — exit if somehow null
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            // STEP 2: Get the focused row as a dynamic object — exit if no row is selected
            dynamic selectedRequest = view.GetRow(view.FocusedRowHandle);
            if (selectedRequest == null) return;

            string studentId = selectedRequest.UserID?.ToString();
            string targetOffice = this.OfficeName;
            string targetStatus = string.Empty;
            string finalRemarks = string.Empty;

            // STEP 3: Determine which button was clicked via its Tag property
            string buttonTag = e.Button.Tag?.ToString();
            switch (buttonTag)
            {
                case "btnApprove":
                    targetStatus = "Approved";
                    finalRemarks = $"Approved by {targetOffice} Office";
                    break;

                case "btnOnHold":
                    targetStatus = "On Hold";
                    finalRemarks = $"On Hold by {targetOffice} Office";
                    break;

                default:
                    return;
            }

            // STEP 4: Confirm before executing — prevents accidental clicks
            string confirmMessage = $"Set this student's clearance to '{targetStatus}'?";
            if (XtraMessageBox.Show(confirmMessage, "Confirm Action",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            // STEP 5: Execute the status update via repository
            ClearanceRepository repo = new ClearanceRepository();
            bool isSuccess = repo.UpdateRequestStatus(studentId, targetOffice, targetStatus, finalRemarks);

            if (isSuccess)
            {
                XtraMessageBox.Show($"Clearance status updated to '{targetStatus}' successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // STEP 6: Update the in-memory row directly so the grid reflects
                // the change immediately without a full reload
                selectedRequest.Status = targetStatus;
                selectedRequest.Remarks = finalRemarks;
                view.RefreshRow(view.FocusedRowHandle);

                // STEP 7: Refresh the dashboard stat cards so CLEARED/PENDING/ON HOLD
                // numbers update live right after this action
                LoadDashboardStats();
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

        // ───────────────────────────────────────────────────────────────
        // METHOD: LoadDashboardStats
        //
        // CALLED BY: OnLoad() and sbOfficeDashboard_Click()
        //
        // PURPOSE:
        //   Reads live counts from ClearanceRepository and updates
        //   the 3 stat card number labels + progress bar text
        //   on the office Dashboard page.
        //
        // USES:
        //   this.OfficeName → set by each child form (SSG/Treasurer/Technical)
        //   so each office only sees counts for their own department
        //
        // LABELS UPDATED:
        //   lblStatCleared  → count of Approved rows for this office
        //   lblStatPending  → count of Pending rows for this office
        //   lblStatOnHold   → count of On Hold rows for this office
        //   labelControl6   → "X out of Y students cleared" progress text
        //   progressBarControl1 → fills proportionally (cleared / total)
        // ───────────────────────────────────────────────────────────────
        protected void LoadDashboardStats()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

                // Get counts per status for this office only
                int cleared = repo.GetStatusCountForOffice(this.OfficeName, "Approved");
                int pending = repo.GetStatusCountForOffice(this.OfficeName, "Pending");
                int onHold = repo.GetStatusCountForOffice(this.OfficeName, "On Hold");
                int total = repo.GetTotalStudentsForOffice(this.OfficeName);

                // Update the stat card number labels
                lblStatCleared.Text = cleared.ToString();
                lblStatPending.Text = pending.ToString();
                lblStatOnHold.Text = onHold.ToString();

                // Update progress bar text and fill level
                labelControl6.Text = $"{cleared} out of {total} students cleared";

                if (total > 0)
                    progressBarControl1.Position = (cleared * 100) / total;
                else
                    progressBarControl1.Position = 0;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not load dashboard stats: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
    }
}