using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class BaseOfficeForm : XtraForm
    {
        private readonly UserRepository repo = new UserRepository();

        public string OfficeName { get; set; }

        public BaseOfficeForm()
        {
            InitializeComponent();
        }

        private void BaseOfficeForm_Load(object sender, EventArgs e)
        {
            SetupIdentity();
            LoadDashboardData();
        }

        private void SetupIdentity()
        {
            if (Session.CurrentUser != null)
            {
             lblFullName.Text = Session.CurrentUser.FullName;
             lblRole.Text = Session.CurrentUser.Role;

              this.OfficeName = Session.CurrentUser.Role;

              this.Text = $"{Session.CurrentUser.Role} Dashboard - {Session.CurrentUser.FullName}";
            }
        }

        private void LoadDashboardData()
        {
            if (string.IsNullOrEmpty(this.OfficeName)) return;

            var requests = repo.GetDepartmentRequests(this.OfficeName);
            gridControl1.DataSource = requests;

            if (requests != null)
            {
             var requestList = requests.ToList();
             int totalRequests = requestList.Count;

             int clearedCount = requestList.Count(r => r.Status != null && r.Status.Equals("Cleared", StringComparison.OrdinalIgnoreCase));
             int pendingCount = requestList.Count(r => r.Status != null && r.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase));

                
            labelControl7.Text = clearedCount.ToString();
            labelControl8.Text = pendingCount.ToString();

             progressBarControl1.Position = totalRequests > 0 ? (clearedCount * 100) / totalRequests : 0;
             labelControl5.Text = $"{clearedCount} out of {totalRequests} students cleared";


            }
        }

        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadDashboardData();
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

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void gridView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view == null || !view.IsDataRow(e.FocusedRowHandle)) return;

            string department = view.GetRowCellValue(e.FocusedRowHandle, "Department")?.ToString();
            string status = view.GetRowCellValue(e.FocusedRowHandle, "Status")?.ToString();
            string remarks = view.GetRowCellValue(e.FocusedRowHandle, "Remarks")?.ToString();

            XtraMessageBox.Show($"Selected Department: {department}\nStatus: {status}\nRemarks: {remarks}",
                                 "Row Details",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Information);
        }
    }
}