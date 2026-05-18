using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    /// <summary>
    /// OOP CONCEPT: INHERITANCE (Code Reusability)
    /// 'TechnicalOffice' inherits directly from 'BaseOfficeForm'. 
    /// It reuses the master data layout engine while injecting its unique department string identifier.
    /// </summary>
    public partial class TechnicalOffice : BaseOfficeForm
    {
        public TechnicalOffice()
        {
            InitializeComponent();

            // FIX: Explicitly assign the identity token string.
            // This MUST match the exact string injected into the database by the StudentPortal ("Technical")
            this.OfficeName = "Technical";

            // FIX: Invoke the centralized parent method to execute the Dapper query 
            // and populate your DevExpress GridControl layout instantly.
            LoadPendingClearanceRequests();
        }
    }
}