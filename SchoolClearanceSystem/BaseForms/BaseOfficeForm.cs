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
        public string OfficeName { get; set; }
        public BaseOfficeForm()
        {
            InitializeComponent();
        }

        private void BaseOfficeForm_Load(object sender, EventArgs e)
        {
            SetupIdentity();
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


        private void sbOfficeDashboard_Click(object sender, EventArgs e)
        {
            naviframeOffices.SelectedPage = pageOfficeDashboard;
            LoadRequest();
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

        protected void LoadRequest()
        {
            UserRepository repo = new UserRepository();

            // Dapper returns an IEnumerable (list) of objects. 
            // DevExpress GridControl handles this much better than a DataTable!
            var requests = repo.GetDepartmentRequests(this.OfficeName);

            gridControl1.DataSource = requests;
        }


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Are you sure you want to sign out?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.CurrentUser = null;
                this.Hide();



                this.Close();
            }
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(
        "Are you sure you want to logout?",
        "Logout",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question );

            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();

                this.Hide(); 
            }
        }
    }
}