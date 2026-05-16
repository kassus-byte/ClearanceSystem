using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SchoolClearanceSystem.Models;
using SchoolClearanceSystem.Repository;


namespace SchoolClearanceSystem
{
    public partial class StudentPortal : DevExpress.XtraEditors.XtraForm
    {


        public StudentPortal()
        {
            InitializeComponent();
            UpdateDashboard();
        }


        private void sbDashboard_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageDashboard;
            UpdateDashboard();
        }

        private void sbRequestClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageRequestClearance;
        }

        private void sbMyRequest_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyRequest;
        }

        private void sbMyClearance_Click_1(object sender, EventArgs e)
        {
            naviframeStudent.SelectedPage = pageMyClearance;
        }

        private void UpdateDashboard()
        {
            if (Session.CurrentUser == null)
            {
                return;
            }

            UserRepository db = new UserRepository();
            int cleared = db.GetClearedCount(Session.CurrentUser.UserID);

            lblOfficeCleared.Text = $"Offices Cleared: {cleared}/3";
            int percentage = (cleared * 100) / 3;
            lblPercentage.Text = $"{percentage}%";

            pbOverallProgress.Position = percentage;
            lblStatus.Text = (cleared == 3) ? "Cleared" : "In Progress";
            lblProgress.Text = $"{cleared} out of 3 offices cleared";

            RefreshOfficeStatus();
        }

        private void RefreshOfficeStatus()
        {
            if (Session.CurrentUser == null) return;

            UserRepository repo = new UserRepository();

            var statusList = repo.GetStudentStatus(Session.CurrentUser.UserID);

            XtraMessageBox.Show($"Rows found: {System.Linq.Enumerable.Count(statusList)}");

            gridControlOfficeStatus.DataSource = statusList;
        }

        private void gridView2_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "Status" && e.CellValue != null)
            {
                string status = e.CellValue?.ToString();
                if (status == "Approved")
                {
                    e.Appearance.ForeColor = Color.ForestGreen;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);

                }
                else if (status == "Pending")
                {
                    e.Appearance.ForeColor = Color.Gray;

                }

            }
        }



        private void panelUpload1_Paint(object sender, PaintEventArgs e)
        {
            // Set the color and dash pattern
            Color dashedColor = Color.FromArgb(100, 180, 150); // Muted green
            float[] dashValues = { 5, 3 }; // 5 pixels line, 3 pixels space

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Pen pen = new Pen(dashedColor, 1))
            {
                pen.DashPattern = dashValues;

                // Draw a rounded rectangle or standard rectangle
                // Subtract 1 from width/height to ensure the border isn't clipped
                e.Graphics.DrawRectangle(pen, 0, 0, panelUpload1.Width - 1, panelUpload1.Height - 1);
            }
        }


        private void panelUpload1_MouseClick(object sender, MouseEventArgs e)
        {
            using (XtraOpenFileDialog fileDialog = new XtraOpenFileDialog())
            {
                fileDialog.Filter = "Image Files|*.jpg;*.png|PDF Files|*.pdf";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Logic to handle the file
                    MessageBox.Show("File selected: " + fileDialog.FileName);
                }
            }
        }

        private void panelUpload1_MouseEnter(object sender, EventArgs e)
        {
            panelUpload1.Cursor = Cursors.Hand;
            // Optional: Change BackColor slightly to show hover effect
            panelUpload1.BackColor = Color.FromArgb(250, 255, 250);
        }
    }
}
