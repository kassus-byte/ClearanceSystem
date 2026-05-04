using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class AdminDashboard : DevExpress.XtraEditors.XtraForm
    {
        // Global instance of your Database Manager
        DatabaseManager db = new DatabaseManager();

        public AdminDashboard()
        {
            InitializeComponent();


        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            // pgDashboard is your Navigation Frame
            // The syntax is: [FrameName].SelectedPage = [PageName];
            mainNavigationFrame.SelectedPage = pageDashboard;
        }

        private void btnAccountManagement_Click(object sender, EventArgs e)
        {
            mainNavigationFrame.SelectedPage = pageAccountManagement;


        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }
    }
    }
