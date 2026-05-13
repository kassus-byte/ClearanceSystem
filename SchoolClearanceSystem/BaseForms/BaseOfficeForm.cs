using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem
{
    public partial class BaseOfficeForm : XtraForm
    {
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
    }
}