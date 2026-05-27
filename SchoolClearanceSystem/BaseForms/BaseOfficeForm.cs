using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using SchoolClearanceSystem.Helpers;
using SchoolClearanceSystem.Repository;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            txtSearch.EditValue = null;
            txtSearch.Properties.NullValuePrompt = "Search by Name, ID, or Course..";
            txtSearch.Properties.NullValuePromptShowForEmptyValue = true;
            txtSearch.TextChanged += (s, e) => ApplyUnifiedFilter();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode) return;

            var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null)
            {

                view.OptionsFind.HighlightFindResults = true;
                view.OptionsFind.AllowFindPanel = false;
            }

            btnAllFilter.Click += (s, ev) => SetStatusFilter("All");
            btnPendingFilter.Click += (s, ev) => SetStatusFilter("Pending");
            btnApprovedFilter.Click += (s, ev) => SetStatusFilter("Approved");
            btnOnHoldFilter.Click += (s, ev) => SetStatusFilter("On Hold");

            SetupIdentity();
            LoadPendingClearanceRequests();
            LoadDashboardStats();

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

        private void btnExpandRequest_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeClearanceRequest;
        }

        // ── Data Loading ──────────────────────────────────────────────
        protected void LoadPendingClearanceRequests()
        {
            try
            {
                var data = _repo.GetRequestsForOffice(OfficeName);
                gcBaseOfficeForm.DataSource = data;
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
                pbClearanceProgress.Position = total > 0 ? Math.Min((cleared * 100) / total, 100) : 0;

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
            try
            {
                var view = gcBaseOfficeForm.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                if (view == null) return;

                var data = _repo.GetRequestsForOffice(OfficeName);
                string search = txtSearch.Text.Trim();

                var statusFiltered = data.Where(r =>
                {
                    return _statusFilter == "All" ||
                           (r.Status?.ToString().Trim().Equals(_statusFilter, StringComparison.OrdinalIgnoreCase) == true);
                }).ToList();

                gcBaseOfficeForm.DataSource = statusFiltered;
                view.ApplyFindFilter(search);
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not apply filter: {ex.Message}");
            }
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

                ApplyUnifiedFilter();
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
                    remarks = $"Please visit the {OfficeName} Office to resolve your clearance hold";
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

            DocumentService.ViewDocument(selected.FilePath?.ToString());
        }

        // ── Logout ────────────────────────────────────────────────────
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            if (UIHelper.ShowConfirmation("Are you sure you want to logout?", "Logout") != DialogResult.Yes) return;
            Session.CurrentUser = null;
            new Login().Show();
            this.Close();
        }

        private void labelControl17_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            // 1. Instantiate your report layout
            OfficeReport myReport = new OfficeReport();

            // 2. Fetch data from your database using your UI element filters
            string selectedSemester = cmbSemester.Text;
            string selectedYear = txtAcademicYear.Text;
            string selectedStatus = cmbStatus.Text;

            // TODO: Connect this to your actual database fetching method
            var studentClearanceData = GetClearanceDataFromDatabase(selectedSemester, selectedYear, selectedStatus);
            myReport.DataSource = studentClearanceData;

            // 3. Manual Binding (Mapping data columns to your report table cells)
            myReport.xrTableCell1.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[student_id]"));
            myReport.xrTableCell2.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[student_name]"));
            myReport.xrTableCell3.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[status]"));

            // 4. Generate the document structure in the background
            myReport.CreateDocument();

            // 5. Define where to save the temporary PDF file
            // This saves it to the user's Documents folder with a unique filename
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pdfFilePath = Path.Combine(documentsPath, $"Clearance_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            // 6. Export the report to PDF silently
            myReport.ExportToPdf(pdfFilePath);

            // 7. Open the PDF immediately using the system's default PDF viewer (like Adobe or Chrome)
            Process.Start(new ProcessStartInfo
            {
                FileName = pdfFilePath,
                UseShellExecute = true // Ensures it opens the external application
            });
        }
    }
    }
}