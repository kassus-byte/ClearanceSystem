using DevExpress.XtraEditors;
using System.Data;
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

        private readonly ClearanceRepository _repo = new ClearanceRepository();
        private readonly SystemRepository _sysRepo = new SystemRepository();

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
            LoadActivePeriod();
            LoadPendingClearanceRequests();
            LoadDashboardStats();

            cmbArchiveSemester.SelectedIndexChanged += cmbArchiveSemester_SelectedIndexChanged;
            cmbArchiveYear.SelectedIndexChanged += cmbArchiveYear_SelectedIndexChanged;
        }

        private string _semester = "Not Set";
        private string _academicYear = "Not Set";

        private void LoadActivePeriod()
        {
            try
            {
                var active = _sysRepo.GetActivePeriodSettings();
                if (active != null)
                {
                    _semester = active.Semester ?? "Not Set";
                    _academicYear = active.AcademicYear ?? "Not Set";
                }
                else
                {
                    _semester = "Not Set";
                    _academicYear = "Not Set";
                }
            }
            catch
            {
                _semester = "Not Set";
                _academicYear = "Not Set";
            }

            UIHelper.SetPeriodFields(lblSemester, lblAcademicYear, _semester, _academicYear);
        }

        // ── Identity ──────────────────────────────────────────────────
        // ── Identity ──────────────────────────────────────────────────
        private void SetupIdentity()
        {
            if (Session.CurrentUser == null) return;

            lblFullName.Text = Session.CurrentUser.FullName;
            lblRole.Text = Session.CurrentUser.Role;
            this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";

            // Change the main header text dynamically depending on the current office/role
            // Note: If your control name in the designer is 'lblWelcome' or 'txtWelcome', 
            // change the variable name below to match it exactly.
            if (!string.IsNullOrEmpty(OfficeName) && OfficeName != "Unknown Office")
            {
                txtWelcome.Text = $"{OfficeName} Dashboard";
            }
            else if (!string.IsNullOrEmpty(Session.CurrentUser.Role))
            {
                txtWelcome.Text = $"{Session.CurrentUser.Role} Dashboard";
            }
            else
            {
                txtWelcome.Text = "Office Dashboard";
            }
        }

        // ── Navigation ────────────────────────────────────────────────
        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadPendingClearanceRequests();
            LoadActivePeriod();
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
                var data = _repo.GetRequestsForOffice(OfficeName, _semester, _academicYear);
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
                int cleared = _repo.GetStatusCountForOffice(OfficeName, "Approved", _semester, _academicYear);
                int pending = _repo.GetStatusCountForOffice(OfficeName, "Pending", _semester, _academicYear);
                int onHold = _repo.GetStatusCountForOffice(OfficeName, "On Hold", _semester, _academicYear);
                int total = _repo.GetTotalStudentsForOffice(OfficeName, _semester, _academicYear);

                lblStatCleared.Text = cleared.ToString();
                lblStatPending.Text = pending.ToString();
                lblStatOnHold.Text = onHold.ToString();
                lblProgressSummary.Text = $"{cleared} out of {total} students cleared";
                pbClearanceProgress.Position = total > 0 ? Math.Min((cleared * 100) / total, 100) : 0;

                gcRecentRequests.DataSource = _repo
                    .GetRecentRequestsForOffice(OfficeName, 10, _semester, _academicYear)
                    .ToList();
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

                var data = _repo.GetRequestsForOffice(OfficeName, _semester, _academicYear);
                string search = txtSearch.Text.Trim();

                var statusFiltered = data.Where(r =>
                    _statusFilter == "All" ||
                    (r.Status?.ToString().Trim().Equals(_statusFilter, StringComparison.OrdinalIgnoreCase) == true)
                ).ToList();

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

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                OfficeReport myReport = new OfficeReport();

                string selectedSemester = cmbSemester.Text;
                string selectedYear = txtAcademicYear.Text;
                string selectedStatus = cmbStatus.Text;

                DataTable reportData = _repo.GetClearanceReportData(selectedSemester, selectedYear, selectedStatus, OfficeName);
                myReport.DataSource = reportData;

                myReport.lblReportYear.Text = selectedYear;
                myReport.lblReportSem.Text = selectedSemester;
                myReport.lblReportStatus.Text = selectedStatus;

                myReport.cellReportID.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[student_id]"));
                myReport.cellReportName.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[student_name]"));
                myReport.cellReportProgram.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[program]"));
                myReport.cellReportYearLevel.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[year]"));

                myReport.CreateDocument();

                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string pdfFilePath = Path.Combine(documentsPath, $"Clearance_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                myReport.ExportToPdf(pdfFilePath);

                Process.Start(new ProcessStartInfo
                {
                    FileName = pdfFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show($"Failed to generate office clearance report: {ex.Message}",
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Archive Management ────────────────────────────────────────
        private void sbOfficeArchives_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeArchive;
            LoadArchiveFilters();
        }

        private void LoadArchiveFilters()
        {
            var periods = _sysRepo.GetAllPeriods().ToList();

            // Populate Semester combo box (Strictly genuine distinct data)
            var semesters = periods.Select(p => p.Semester).Distinct().ToList();
            cmbArchiveSemester.Properties.Items.Clear();
            cmbArchiveSemester.Properties.Items.AddRange(semesters);
            cmbArchiveSemester.SelectedIndex = semesters.Count > 0 ? 0 : -1;

            // Populate Academic Year combo box (Strictly genuine distinct data, newest first)
            var years = periods.Select(p => p.AcademicYear).Distinct().OrderByDescending(y => y).ToList();
            cmbArchiveYear.Properties.Items.Clear();
            cmbArchiveYear.Properties.Items.AddRange(years);
            cmbArchiveYear.SelectedIndex = years.Count > 0 ? 0 : -1;
        }

        private void LoadArchiveGrid()
        {
            try
            {
                string sem = cmbArchiveSemester.Text.Trim();
                string year = cmbArchiveYear.Text.Trim();

                // Stop execution if either criteria parameter is empty or unselected
                if (string.IsNullOrEmpty(sem) || string.IsNullOrEmpty(year))
                {
                    gcOfficeArchive.DataSource = null;
                    return;
                }

                // Query the database strictly searching for the specific selected sem and year combination
                var data = _repo.GetArchivedRequests(sem, year, OfficeName).ToList();
                gcOfficeArchive.DataSource = data;
            }
            catch (Exception ex)
            {
                UIHelper.ShowError($"Could not load archive: {ex.Message}");
            }
        }

        // Auto-filter grid immediately when selection changes
        private void cmbArchiveSemester_SelectedIndexChanged(object sender, EventArgs e) => LoadArchiveGrid();
        private void cmbArchiveYear_SelectedIndexChanged(object sender, EventArgs e) => LoadArchiveGrid();

        // Manual button execution trigger
        private void btnViewRecord_Click(object sender, EventArgs e) => LoadArchiveGrid();
    }
}