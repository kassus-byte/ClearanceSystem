using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;


namespace SchoolClearanceSystem
{
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {


        public StudentPortal()
        {
            InitializeComponent();
            UpdateDashboard();
        }

       
        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;
        }

        private void sbMyClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;
        }

        private void UpdateDashboard()
        {
            DatabaseManager db = new DatabaseManager();
            int cleared = db.GetClearedCount(Session.CurrentUser.UserID);

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            int percentage = (cleared * 100) / 3;
            lblPercentage.Text = $"{percentage}%";

            pbOverallProgress.Position = percentage;
            lblStatus.Text = (cleared == 3)? "Cleared" : "In Progress";
            lblProgress.Text = $"{cleared} out of 3 offices cleared"; 
        }
    }
}
