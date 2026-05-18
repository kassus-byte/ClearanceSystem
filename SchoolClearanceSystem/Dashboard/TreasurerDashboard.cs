using DevExpress.XtraEditors;
using System;

namespace SchoolClearanceSystem.Dashboard
{
    /// <summary>
    /// OOP CONCEPT: INHERITANCE (Code Reusability)
    /// 'TreasurerDashboard' inherits directly from 'BaseOfficeForm'. 
    /// Instead of rewriting navigation layouts, user welcome messages, and grid controllers, 
    /// it simply reuses the base architecture and passes its unique identity token ("Treasurer").
    /// </summary>
    public partial class TreasurerDashboard : BaseOfficeForm
    {
        public TreasurerDashboard()
        {
            InitializeComponent();

            // OOP CONCEPT: POLYMORPHISM / STATE INITIALIZATION
            // Setting this property tells the base form's database query exactly 
            // which department records to look up when Dapper runs.
            this.OfficeName = "Treasurer";

            // Calls the centralized base class method to automatically pull 
            // and populate the grid control layout with Treasurer requests.
            LoadPendingClearanceRequests();
        }
    }
}