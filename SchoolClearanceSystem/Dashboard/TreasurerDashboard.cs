using DevExpress.XtraEditors;
using SchoolClearanceSystem.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace SchoolClearanceSystem.Dashboard
{
    public partial class TreasurerDashboard : DevExpress.XtraEditors.XtraForm
    {
        // 1. Declare the DatabaseManager at the class level
        DatabaseManager db = new DatabaseManager();

        public TreasurerDashboard()
        {
            InitializeComponent();
        }

        private void TreasurerDashboard_Load(object sender, EventArgs e)
        {

            try
            {
                // Fetch data from DB
                DataTable dt = db.GetDepartmentRequests("Treasurer");

                // We check if dt is null to avoid crashes, but we'll try to bind regardless
                if (dt != null)
                {
                    gcTreasurer.DataSource = dt;
                    gcTreasurer.ForceInitialize();

                    // --- THE MAGIC FIX ---
                    // This ignores your manual designer settings and 
                    // creates columns based on the Database column names.
                    gridView1.PopulateColumns();

                    gridView1.BestFitColumns();

                    // Only show a message if there are actually 0 requests
                    if (dt.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("No pending requests found for the Treasurer.");
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Error: " + ex.Message);
            }
        }
    }
    }
    
