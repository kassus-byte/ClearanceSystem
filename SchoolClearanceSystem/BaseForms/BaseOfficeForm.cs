using DevExpress.XtraEditors;
using SchoolClearanceSystem.Helpers;
using SchoolClearanceSystem.Repository;
using System;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class BaseOfficeForm : XtraForm
    {
        private string _statusFilter = "All";
        public string OfficeName { get; set; } = "Unknown Office";

        // Single shared repo instance — no need to create a new one per method call
        private readonly ClearanceRepository _repo = new ClearanceRepository();

        public BaseOfficeForm()
        {
            InitializeComponent();
            txtSearch.TextChanged += (s, e) => ApplyUnifiedFilter();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (DesignMode) { base.OnLoad(e); return; }

            btnAllFilter.Click += (s, ev) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, ev) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, ev) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, ev) => SetStatusFilter("On Hold");

            SetupIdentity();
            LoadPendingClearanceRequests();
            LoadDashboardStats();
            base.OnLoad(e);
        }

        // ── Identity ──────────────────────────────────────────────────
        private void SetupIdentity()
        {
            if (Session.CurrentUser == null) return;
            lblFullName.Text = Session.CurrentUser.FullName;
            lblRole.Text = Session.CurrentUser.Role;
            this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
        }

        // ── Navigation ────────────────────────────────────────────────
        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadPendingClearanceRequests();
            LoadDashboardStats();
        }

        private void sbOfficeClearanceRequest_Click_1(object sender, EventArgs e) =>
            naviframeOffices.SelectedPage = pageOfficeClearanceRequest;

        private void sbOfficeReports_Click_1(object sender, EventArgs e) =>
            naviframeOffices.SelectedPage = pageOfficeReports;

        // ── Data Loading ──────────────────────────────────────────────
        protected void LoadPendingClearanceRequests()
        {
            try
            {
                gcBaseOfficeForm.DataSource = _repo.GetRequestsForOffice(OfficeName);
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load office requests: {ex.Message}", "Data Error");
            }
        }

        protected void LoadDashboardStats()
        {
            try
            {
                int cleared = _repo.GetStatusCountForOffice(OfficeName, "Approved");
                int pending = _repo.GetStatusCountForOffice(OfficeName, "Pending");
                int onHold = _repo.GetStatusCountForOffice(OfficeName, "On Hold");
                int total = _repo.GetTotalStudentsForOffice(OfficeName);

                lblStatCleared.Text = cleared.ToString();
                lblStatPending.Text = pending.ToString();
                lblStatOnHold.Text = onHold.ToString();
                lblProgressSummary.Text = $"{cleared} out of {total} students cleared";
                pbClearanceProgress.Position = total > 0 ? (cleared * 100) / total : 0;
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load dashboard stats: {ex.Message}");
            }
        }

        // ── Filtering ─────────────────────────────────────────────────
        private void SetStatusFilter(string status)
        {
            _statusFilter = status;
            ApplyUnifiedFilter();
        }

        private void ApplyUnifiedFilter()
        {
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            string filter = _statusFilter != "All" ? $"[Status] = '{_statusFilter}'" : string.Empty;

            string search = txtSearch.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrEmpty(search))
            {
                string searchPart = $"([UserID] LIKE '%{search}%' OR [FullName] LIKE '%{search}%' OR [Program] LIKE '%{search}%')";
                filter = string.IsNullOrEmpty(filter) ? searchPart : $"{filter} AND {searchPart}";
            }

            view.ActiveFilterString = filter;
        }

        // ── Actions ───────────────────────────────────────────────────
        private void btnAction_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            dynamic selected = view.GetRow(view.FocusedRowHandle);
            if (selected == null) return;

            string studentId = selected.UserID?.ToString();
            string tag = e.Button.Tag?.ToString();

            if (!TryResolveAction(tag, out string status, out string remarks)) return;

            if (UIHelper.ShowConfirmation($"Set this student's clearance to '{status}'?", "Confirm Action") != DialogResult.Yes) return;

            bool ok = _repo.UpdateRequestStatus(studentId, OfficeName, status, remarks);
            if (ok)
            {
                UIHelper.ShowSuccess($"Clearance status updated to '{status}' successfully!");
                selected.Status = status;
                selected.Remarks = remarks;
                view.RefreshRow(view.FocusedRowHandle);
                LoadDashboardStats();
            }
            else
            {
                UIHelper.ShowError("Database update failed. Check connection.");
            }
        }

        private bool TryResolveAction(string tag, out string status, out string remarks)
        {
            switch (tag)
            {
                case "btnApprove":
                    status = "Approved";
                    remarks = $"Approved by {OfficeName} Office";
                    return true;
                case "btnOnHold":
                    status = "On Hold";
                    remarks = $"On Hold by {OfficeName} Office";
                    return true;
                default:
                    status = remarks = string.Empty;
                    return false;
            }
        }

        private void btnProof_Click(object sender, EventArgs e)
        {
            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null) return;

            dynamic selected = view.GetRow(view.FocusedRowHandle);
            if (selected == null) return;

            DocumentService.ViewDocument(selected.Proof?.ToString());
        }

        // ── Logout ────────────────────────────────────────────────────
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmation("Are you sure you want to logout?", "Logout") != DialogResult.Yes) return;
            Session.CurrentUser = null;
            new Login().Show();
            this.Close();
        }
    }
}