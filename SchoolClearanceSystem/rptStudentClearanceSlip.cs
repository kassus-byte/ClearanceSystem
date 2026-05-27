using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace SchoolClearanceSystem
{
    public partial class rptStudentClearanceSlip : DevExpress.XtraReports.UI.XtraReport
    {
        public rptStudentClearanceSlip()
        {
            InitializeComponent();
        }

        public void InitData(object studentProfile, string technicalStatus, string ssgStatus, string treasurerStatus, string currentSemester, string currentSchoolYear)
        {
            this.DataSource = studentProfile;

            cellFullName.DataBindings.Add("Text", null, "FullName");
            cellStudentID.DataBindings.Add("Text", null, "UserID");
            cellYearLevel.DataBindings.Add("Text", null, "Year");

            cellSem.Text = currentSemester;
            cellSchoolYear.Text = currentSchoolYear;

            cellStatusTechnical.Text = technicalStatus;
            cellStatusSSG.Text = ssgStatus;
            cellStatusTreasurer.Text = treasurerStatus;

            ApplyStatusStyle(cellStatusTechnical);
            ApplyStatusStyle(cellStatusSSG);
            ApplyStatusStyle(cellStatusTreasurer);
        }

        private void ApplyStatusStyle(XRLabel statusLabel)
        {
            if (statusLabel.Text == "Cleared")
            {
                statusLabel.ForeColor = Color.FromArgb(0, 90, 70);

            }

            else
            {
                statusLabel.ForeColor = Color.FromArgb(180, 40, 40);
            }



        }
    }
}
