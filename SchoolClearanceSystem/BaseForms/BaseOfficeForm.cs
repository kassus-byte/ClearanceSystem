using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                int onHold = requestList.Count(r => r.Status != null && r.Status.Equals("On Hold", StringComparison.OrdinalIgnoreCase));
                labelControl7.Text = clearedCount.ToString();
                labelControl8.Text = pendingCount.ToString();
                labelControl9.Text = onHold.ToString();

                progressBarControl1.Position = totalRequests > 0 ? (clearedCount * 100) / totalRequests : 0;
                labelControl5.Text = $"{clearedCount} out of {totalRequests} students cleared";

                labelControl5.Text = $"{clearedCount} out of {totalRequests} students cleared";

                if (totalRequests > 0)
                {
                   // bar's scale to match your real student count dynamically
                    progressBarControl1.Properties.Minimum = 0;
                    progressBarControl1.Properties.Maximum = totalRequests;

                 //block fill position to the exact number of cleared students
                    progressBarControl1.Position = clearedCount;
                }
                else
                {
                    progressBarControl1.Position = 0;
                }
              
            }
        }

            
            
        

        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadDashboardData();
        }

        //Clearance
        private async void sbOfficeClearanceRequest_Click_1(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeClearanceRequest;

            try
            {

                await LoadClearanceRequestsAsync();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Failed to load clearance requests: {ex.Message}", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadClearanceRequestsAsync()
        {

            string connectionString = "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;";
            string query = "SELECT RequestID, EmployeeName, Department, Status, RequestDate FROM OfficeClearanceRequests WHERE Status = 'Pending'";

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();

                    await connection.OpenAsync();

                    using (System.Data.SqlClient.SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }


                    gridControl2.DataSource = dataTable;
                }
            }
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
        //Cleared
        private void panelControl6_Paint(object sender, PaintEventArgs e)
        {

        }
        //
        //Pending
        private void panelControl7_Paint(object sender, PaintEventArgs e)
        {

        }


        //Hold
        private void panelControl8_Paint(object sender, PaintEventArgs e)
        {

        }

        //
        //
        //Bar
        private void progressBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            var progressBar = sender as DevExpress.XtraEditors.ProgressBarControl;
            if (progressBar == null) return;

            
            if (progressBar.Position > 0 && progressBar.Position >= progressBar.Properties.Maximum)
            {
                progressBar.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                progressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = false;

                progressBar.Properties.StartColor = System.Drawing.Color.ForestGreen;
                progressBar.Properties.EndColor = System.Drawing.Color.ForestGreen;
            }
            else
            {
               
                progressBar.Properties.LookAndFeel.UseDefaultLookAndFeel = true;
            }
        }

       
    }
}