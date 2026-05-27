using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace SchoolClearanceSystem
{
    public partial class OfficeReport : DevExpress.XtraReports.UI.XtraReport
    {
        public OfficeReport()
        {
            InitializeComponent();

            this.xrTableCell1.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[student_id]"));
            this.xrTableCell2.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[fullname]"));
            this.xrTableCell3.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[status]"));
        }

    }
}
