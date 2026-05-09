using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {


        public StudentPortal()
        {
            InitializeComponent();

        }

       
        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
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
    }
}
