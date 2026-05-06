using DevExpress.XtraEditors;
using SchoolClearanceSystem.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class TreasurerDashboard : BaseClearanceForm
    {
        public TreasurerDashboard()
        {
            InitializeComponent();
            this.CurrentOffice = "Treasurer"; // The Key for the Database
        }

        private void TreasurerDashboard_Load(object sender, EventArgs e) => LoadGridData();


    }
}

    


