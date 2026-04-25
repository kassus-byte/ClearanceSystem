using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {
        public StudentPortal()
        {
            InitializeComponent();
        }

      

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
                if (e.Column.FieldName == "Status")
                {
                    string status = e.CellValue?.ToString();

                    switch (status)
                    {
                        case "Cleared":
                            // Green background
                            e.Appearance.BackColor = ColorTranslator.FromHtml("#EAF3DE");
                            e.Appearance.ForeColor = ColorTranslator.FromHtml("#27500A");
                            break;

                        case "Pending":
                            // Amber background
                            e.Appearance.BackColor = ColorTranslator.FromHtml("#FAEEDA");
                            e.Appearance.ForeColor = ColorTranslator.FromHtml("#633806");
                            break;

                        case "Hold":
                            // Red background
                            e.Appearance.BackColor = ColorTranslator.FromHtml("#FCEBEB");
                            e.Appearance.ForeColor = ColorTranslator.FromHtml("#791F1F");
                            break;

                        case "In Progress":
                            // Teal background
                            e.Appearance.BackColor = ColorTranslator.FromHtml("#E1F5EE");
                            e.Appearance.ForeColor = ColorTranslator.FromHtml("#085041");
                            break;
                    }

                    // Make the font bold
                    e.Appearance.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                    e.Appearance.TextOptions.HAlignment =
                        DevExpress.Utils.HorzAlignment.Center;
                }
        }

        private void StudentPortal_Load(object sender, EventArgs e)
        {
            pnlDashboard.BringToFront();
            ShowPage(pnlDashboard);
        }
        private void ShowPage(PanelControl pageToShow)
        {
            pnlDashboard.Visible = false;
            pnlRequestClearance.Visible = false;
            pnlMyRequest.Visible = false;
            pnlMyClearance.Visible = false;
            pnlRequirements.Visible = false;
            pnlNotifications.Visible = false;

            pageToShow.Visible = true;
        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlDashboard);
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlRequestClearance);
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlMyRequest);
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlMyClearance);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlRequirements);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ShowPage(pnlNotifications);
        }
    }
}
