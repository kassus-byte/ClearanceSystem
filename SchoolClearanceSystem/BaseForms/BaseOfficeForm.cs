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
            if (this.DesignMode) { base.OnLoad(e); return; }

            btnAllFilter.Click += (s, ev) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, ev) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, ev) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, ev) => SetStatusFilter("On Hold");

            SetupIdentity();
            LoadPendingClearanceRequests();
            LoadDashboardStats(); 
            base.OnLoad(e);
        }

        private void SetupIdentity()
        {
            if (Session.CurrentUser != null)
            {
                lblFullName.Text = Session.CurrentUser.FullName;
                lblRole.Text = Session.CurrentUser.Role;

                
                this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
            }
        }

        #region Navigation Control Flow Routine Managers

        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadPendingClearanceRequests();
            LoadDashboardStats(); 
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

        protected void LoadPendingClearanceRequests()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

             
                var pendingDataList = repo.GetRequestsForOffice(this.OfficeName);

               
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

          
            if (currentStatusFilter != "All")
            {
                filterCriteria = $"[Status] = '{currentStatusFilter}'";
            }

            
            string searchText = txtSearch.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrEmpty(searchText))
            {
                string searchCriteria = $"([UserID] LIKE '%{searchText}%' OR [FullName] LIKE '%{searchText}%' OR [Program] LIKE '%{searchText}%')";

                if (string.IsNullOrEmpty(filterCriteria))
                    filterCriteria = searchCriteria;
                else
                    filterCriteria += $" AND {searchCriteria}";
            }

           
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
            string finalRemarks = string.Empty;

          
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

           
            string confirmMessage = $"Set this student's clearance to '{targetStatus}'?";
            if (XtraMessageBox.Show(confirmMessage, "Confirm Action",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

          
            ClearanceRepository repo = new ClearanceRepository();
            bool isSuccess = repo.UpdateRequestStatus(studentId, targetOffice, targetStatus, finalRemarks);

            if (isSuccess)
            {
                XtraMessageBox.Show($"Clearance status updated to '{targetStatus}' successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                selectedRequest.Status = targetStatus;
                selectedRequest.Remarks = finalRemarks;
                view.RefreshRow(view.FocusedRowHandle);

              
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
           
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

      
            dynamic selectedRequest = view.GetRow(view.FocusedRowHandle);
            if (selectedRequest == null) return;

            try
            {
                string proofPath = selectedRequest.Proof?.ToString();

              
                OpenTargetFile(proofPath);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"File System Sync Error: Unable to extract file tracking structure. {ex.Message}",
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void LoadDashboardStats()
        {
            try
            {
                ClearanceRepository repo = new ClearanceRepository();

                
                int cleared = repo.GetStatusCountForOffice(this.OfficeName, "Approved");
                int pending = repo.GetStatusCountForOffice(this.OfficeName, "Pending");
                int onHold = repo.GetStatusCountForOffice(this.OfficeName, "On Hold");
                int total = repo.GetTotalStudentsForOffice(this.OfficeName);

                
                lblStatCleared.Text = cleared.ToString();
                lblStatPending.Text = pending.ToString();
                lblStatOnHold.Text = onHold.ToString();

              
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