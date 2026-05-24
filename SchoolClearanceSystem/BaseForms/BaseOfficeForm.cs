using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class BaseOfficeForm : XtraForm
    {
        private string currentStatusFilter = "All";
        public string OfficeName { get; set; } = "Unknown Office";

        public BaseOfficeForm()
        {
            InitializeComponent();
            txtSearch.TextChanged += TxtSearch_TextChanged;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
            {
                base.OnLoad(e);
                return;
            }

            // FIX: Set OfficeName from session BEFORE loading any data
            if (Session.CurrentUser != null)
            {
                // Map Role to exact Department value stored in DB
                switch (Session.CurrentUser.Role)
                {
                    case "Registrar": this.OfficeName = "Registrar"; break;
                    case "Treasurer": this.OfficeName = "Treasurer"; break;
                    case "Technical Office": this.OfficeName = "Technical"; break;
                    case "SSG": this.OfficeName = "SSG"; break;
                    default: this.OfficeName = Session.CurrentUser.Role; break;
                }
            }

            btnAllFilter.Click += (s, ev) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, ev) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, ev) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, ev) => SetStatusFilter("On Hold");

            SetupIdentity();
            LoadPendingClearanceRequests();
            LoadDashboardSummary();

            base.OnLoad(e);
        }
        private void SetupIdentity()
        {
            if (Session.CurrentUser != null)
            {
                string firstName = Session.CurrentUser.FullName.Split(' ')[0];

                // Sidebar
                lblFullName.Text = Session.CurrentUser.FullName;
                lblRole.Text = Session.CurrentUser.Role;

                // Top bar — merges role + firstname into the big title
                // Shows: "Registrar Dashboard, Anne!"
                txtWelcome.Text = $"{Session.CurrentUser.Role} Dashboard, {firstName}!";

                // Hide labelControl5 since it has no assignment — it sits in progress section
                lblFirstName.Text = "";

                // Subtitle
                labelControl3.Text = "Clearance Overview";

                // Window caption
                this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
            }
        }
        #region Navigation Control Flow Routine Managers

        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadPendingClearanceRequests();
            LoadDashboardSummary();
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

        protected void LoadPendingClearanceRequests()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

                // 1. Fetch data items matching our workspace identity parameter context
                var pendingDataList = repo.GetRequestsForOffice(this.OfficeName);

                // 2. Assign data items directly to your layout table grid container
                gcBaseOfficeForm.DataSource = pendingDataList;

                // ── FILLED CODE: Call your newly added repository functions ───────
                int cleared = repo.GetRequestCountByStatus(this.OfficeName, "Approved");
                int pending = repo.GetRequestCountByStatus(this.OfficeName, "Pending");
                int onHold = repo.GetRequestCountByStatus(this.OfficeName, "On Hold");

                // 3. Update the UI text counters safely
                if (lblClearedCount != null) lblClearedCount.Text = cleared.ToString();
                if (lblPendingCount != null) lblPendingCount.Text = pending.ToString();
                if (lblOnHoldCount != null) lblOnHoldCount.Text = onHold.ToString();

                // 4. Update progress bar text dynamically
                int totalRequests = cleared + pending + onHold;
                if (lblClearanceProgress != null)
                {
                    lblClearanceProgress.Text = $"{cleared} out of {totalRequests} students cleared";
                }
                // ─────────────────────────────────────────────────────────────────
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Could not bind office requests table rows: {ex.Message}",
                    "Data Retrieval Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void LoadDashboardSummary()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

                int clearedCount = repo.GetRequestCountByStatus(this.OfficeName, "Approved");
                int pendingCount = repo.GetRequestCountByStatus(this.OfficeName, "Pending");
                int onHoldCount = repo.GetRequestCountByStatus(this.OfficeName, "On Hold");

                // Update summary cards — labelControl7=CLEARED, 8=PENDING, 9=ON HOLD
                lblClearedCount.Text = clearedCount.ToString();
                lblPendingCount.Text = pendingCount.ToString();
                lblOnHoldCount.Text = onHoldCount.ToString();

                lblClearedCount.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
                lblPendingCount.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);
                lblOnHoldCount.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold);

                // Progress bar
                int totalRequests = clearedCount + pendingCount + onHoldCount;
                int safeTotal = totalRequests > 0 ? totalRequests : 1;
                int progressValue = (int)((clearedCount / (double)safeTotal) * 100);
                progressValue = Math.Min(progressValue, 100);

                progressBarControl1.Properties.Minimum = 0;
                progressBarControl1.Properties.Maximum = 100;
                progressBarControl1.EditValue = progressValue;

                // labelControl6 = "0 out of X students cleared"
                labelControl6.Text = $"{clearedCount} out of {totalRequests} students cleared";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Dashboard summary could not be loaded: {ex.Message}",
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

            // 1. Status filter
            if (currentStatusFilter != "All")
            {
                filterCriteria = $"[Status] = '{currentStatusFilter}'";
            }

            // 2. Search filter — skip placeholder, filter on bound properties only
            string searchText = txtSearch.Text.Trim().Replace("'", "''");
            bool isPlaceholder = searchText == "Search by Name, ID, or Course..";

            if (!string.IsNullOrEmpty(searchText) && !isPlaceholder)
            {
                string searchCriteria = $"([UserID] LIKE '%{searchText}%' OR [FullName] LIKE '%{searchText}%' OR [Program] LIKE '%{searchText}%')";

                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = searchCriteria;
                else
                    filterCriteria += $" AND {searchCriteria}";
            }

            // 3. Apply to grid
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

                selectedRequest.Status = targetStatus;
                selectedRequest.Remarks = defaultRemarks;

                view.RefreshRow(view.FocusedRowHandle);

                // Refresh dashboard counts after every action
                LoadDashboardSummary();
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
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            dynamic selectedRequest = view.GetRow(view.FocusedRowHandle);
            if (selectedRequest == null) return;

            try
            {
                // Fixed line 230:
                string proofPath = selectedRequest.FilePath?.ToString();
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