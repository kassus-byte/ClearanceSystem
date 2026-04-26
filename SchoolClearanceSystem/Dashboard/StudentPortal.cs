using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {
        // List to manage all pages for easier hiding/showing
        private List<PanelControl> allPages;
        // Instance of DatabaseManager to be used throughout the form
        DatabaseManager db = new DatabaseManager();

        public StudentPortal()
        {
            InitializeComponent();

            // Initialize the list with your panels
            allPages = new List<PanelControl> {
                pnlDashboard, pnlRequestClearance, pnlMyRequest,
                pnlMyClearance, pnlRequirements, pnlNotifications
            };
        }

        private void StudentPortal_Load_1(object sender, EventArgs e)
        {
            // 1. Display Student Information from Session
            if (Session.CurrentUser != null)
            {
                lblFullName.Text = Session.CurrentUser.FullName;
                lblUserID.Text = Session.CurrentUser.UserID;
                lblProgram.Text = Session.CurrentUser.Program;
                lblYear.Text = Session.CurrentUser.Year;
                lblRole.Text = Session.CurrentUser.Role;

                lblFullNameWelcome.Text = Session.CurrentUser.FullName;
            }

            // 2. CHECK IF CLEARANCE SEASON IS ACTIVE
            if (!db.IsClearanceActive())
            {
                btnSubmitRequest.Enabled = false;
                btnSubmitRequest.Text = "Submissions Closed";
                // Note: lblStatusNote was removed as it doesn't exist in your Designer
            }

            // 3. Default View
            ShowPage(pnlDashboard);
        }

        // This method must be inside the class for the Load event and NavBars to see it
        private void ShowPage(PanelControl pageToShow)
        {
            if (allPages == null) return;

            // Hide all pages in one loop
            allPages.ForEach(p => p.Visible = false);

            // Show the selected one
            pageToShow.Visible = true;
            pageToShow.Dock = DockStyle.Fill;
            pageToShow.BringToFront();
        }

        #region Navigation
        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlDashboard);
        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlRequestClearance);
        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlMyRequest);
        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlMyClearance);
        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlRequirements);
        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e) => ShowPage(pnlNotifications);
        #endregion

        #region Grid Styling
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                string status = e.CellValue.ToString();

                switch (status)
                {
                    case "Cleared": SetCellColor(e, "#EAF3DE", "#27500A"); break;
                    case "Pending": SetCellColor(e, "#FAEEDA", "#633806"); break;
                    case "Hold": SetCellColor(e, "#FCEBEB", "#791F1F"); break;
                    case "In Progress": SetCellColor(e, "#E1F5EE", "#085041"); break;
                }

                e.Appearance.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
        }

        private void SetCellColor(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e, string backHtml, string foreHtml)
        {
            e.Appearance.BackColor = ColorTranslator.FromHtml(backHtml);
            e.Appearance.ForeColor = ColorTranslator.FromHtml(foreHtml);
        }
        #endregion

        private void btnSubmitRequest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbSemester.Text) || string.IsNullOrWhiteSpace(cmbAcademicYear.Text))
            {
                XtraMessageBox.Show("Please select both Semester and Academic Year before submitting.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sID = Session.CurrentUser.UserID;
            string sem = cmbSemester.Text;
            string ay = cmbAcademicYear.Text;

            try
            {
                bool successTreasurer = db.SubmitClearanceRequest(sID, "Treasurer", sem, ay);
                bool successTech = db.SubmitClearanceRequest(sID, "Technical Office", sem, ay);

                if (successTreasurer && successTech)
                {
                    XtraMessageBox.Show("Requests successfully sent to Treasurer and Technical Office.",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ShowPage(pnlMyRequest);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}