using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class BaseOfficeForm : XtraForm
    {
        // Set this in the child form (e.g., "Treasurer")
        public string CurrentOffice { get; set; }
        protected DatabaseManager db = new DatabaseManager();

        public BaseOfficeForm()
        {
            InitializeComponent();
        }

        // --- DATA LOADING ---
        protected void LoadGridData()
        {
            if (!string.IsNullOrEmpty(CurrentOffice))
            {
                // This fills the DevExpress GridControl
                gridClearance.DataSource = db.GetDepartmentRequests(CurrentOffice);
            }
        }

        // --- GRID HELPER ---
        protected string GetSelectedStudentID()
        {
            // DevExpress GridView logic to get the ID of the row clicked
            var view = gridClearance.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (view != null && view.FocusedRowHandle >= 0)
            {
                return view.GetFocusedRowCellValue("StudentID").ToString();
            }
            return null;
        }

        // --- ACTIONS ---
        // 'virtual' so children can change the behavior if they want
        protected virtual void btnApprove_Click(object sender, EventArgs e)
        {
            string id = GetSelectedStudentID();
            if (string.IsNullOrEmpty(id)) return;

            if (db.UpdateRequestStatus(id, CurrentOffice, "Approved", "Cleared"))
            {
                XtraMessageBox.Show("Student Approved successfully!", "Success");
                LoadGridData(); // Refresh the grid
            }
        }

        protected virtual void btnHold_Click(object sender, EventArgs e)
        {
            string id = GetSelectedStudentID();
            if (string.IsNullOrEmpty(id)) return;

            string remark = "You still have a balance. Please see the department treasurer.";

            if (db.UpdateRequestStatus(id, CurrentOffice, "On Hold", remark))
            {
                XtraMessageBox.Show("Student put on hold.", "Notice");
                LoadGridData(); // Refresh the grid
            }
        }
    }
}