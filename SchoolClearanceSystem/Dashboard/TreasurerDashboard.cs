using DevExpress.XtraEditors;
using System;
namespace SchoolClearanceSystem.Dashboard
{
    public partial class TreasurerDashboard : BaseOfficeForm //inheritance for code reusability
    {
        public TreasurerDashboard()
        {
            InitializeComponent();
            this.OfficeName = "Treasurer";
            LoadRequest(); // Now it knows to fetch 'Treasurer' requests
        }

      // private void TreasurerDashboard_Load(object sender, EventArgs e) => LoadGridData();


    }
}

    


