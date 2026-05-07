using DevExpress.XtraEditors;
using System;
namespace SchoolClearanceSystem.Dashboard
{
    public partial class TreasurerDashboard : BaseOfficeForm //inheritance for code reusability
    {
        public TreasurerDashboard()
        {
            InitializeComponent();
            this.CurrentOffice = "Treasurer"; // The Key for the Database
        }

        private void TreasurerDashboard_Load(object sender, EventArgs e) => LoadGridData();


    }
}

    


