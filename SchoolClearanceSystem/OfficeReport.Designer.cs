namespace SchoolClearanceSystem
{
    partial class OfficeReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfficeReport));
            DevExpress.XtraReports.UI.XRWatermark xrWatermark1 = new DevExpress.XtraReports.UI.XRWatermark();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cellReportID = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellReportName = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellReportStatus = new DevExpress.XtraReports.UI.XRTableCell();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblReportStatus = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportSem = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportYear = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cellHeaderID = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellHeaderName = new DevExpress.XtraReports.UI.XRTableCell();
            this.cellHeaderStatus = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 389.5833F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 425F;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnCount;
            this.Detail.Name = "Detail";
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(37.66666F, 10.00001F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(744.3333F, 25F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cellReportID,
            this.cellReportName,
            this.cellReportStatus});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // cellReportID
            // 
            this.cellReportID.Multiline = true;
            this.cellReportID.Name = "cellReportID";
            this.cellReportID.Text = "cellReportID";
            this.cellReportID.Weight = 1.9167024142520113D;
            // 
            // cellReportName
            // 
            this.cellReportName.Multiline = true;
            this.cellReportName.Name = "cellReportName";
            this.cellReportName.Text = "cellReportName";
            this.cellReportName.Weight = 4.50185801262967D;
            // 
            // cellReportStatus
            // 
            this.cellReportStatus.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellReportStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.cellReportStatus.Multiline = true;
            this.cellReportStatus.Name = "cellReportStatus";
            this.cellReportStatus.StylePriority.UseFont = false;
            this.cellReportStatus.StylePriority.UseForeColor = false;
            this.cellReportStatus.Text = "cellReportStatus";
            this.cellReportStatus.Weight = 2.2784107728475447D;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel6,
            this.lblReportStatus,
            this.lblReportSem,
            this.lblReportYear,
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel3,
            this.xrPictureBox1});
            this.ReportHeader.HeightF = 194.7917F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lblReportStatus
            // 
            this.lblReportStatus.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.lblReportStatus.LocationFloat = new DevExpress.Utils.PointFloat(605.6247F, 161.7917F);
            this.lblReportStatus.Multiline = true;
            this.lblReportStatus.Name = "lblReportStatus";
            this.lblReportStatus.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportStatus.SizeF = new System.Drawing.SizeF(123.9583F, 23F);
            this.lblReportStatus.StylePriority.UseFont = false;
            this.lblReportStatus.StylePriority.UseTextAlignment = false;
            this.lblReportStatus.Text = "Status:";
            this.lblReportStatus.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReportSem
            // 
            this.lblReportSem.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.lblReportSem.LocationFloat = new DevExpress.Utils.PointFloat(311.8747F, 161.7917F);
            this.lblReportSem.Multiline = true;
            this.lblReportSem.Name = "lblReportSem";
            this.lblReportSem.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportSem.SizeF = new System.Drawing.SizeF(114.5833F, 23F);
            this.lblReportSem.StylePriority.UseFont = false;
            this.lblReportSem.StylePriority.UseTextAlignment = false;
            this.lblReportSem.Text = "Semester:";
            this.lblReportSem.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReportYear
            // 
            this.lblReportYear.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.lblReportYear.LocationFloat = new DevExpress.Utils.PointFloat(37.66666F, 161.7917F);
            this.lblReportYear.Multiline = true;
            this.lblReportYear.Name = "lblReportYear";
            this.lblReportYear.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportYear.SizeF = new System.Drawing.SizeF(125F, 23F);
            this.lblReportYear.StylePriority.UseFont = false;
            this.lblReportYear.StylePriority.UseTextAlignment = false;
            this.lblReportYear.Text = "Academic Year: ";
            this.lblReportYear.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 17F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(126.0416F, 21.45834F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(206.25F, 36.54166F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "STATUS REPORT";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(126.0415F, 47.58332F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(365.2086F, 41.75F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "DMC COLLEGE FOUNDATION INC.";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(126.0417F, 72.83331F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(365.2084F, 33.41668F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "COLLEGE OF COMPUTER STUDIES";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(126.0417F, 117.7083F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 63.54167F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(37.6667F, 38.54167F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(744.3333F, 25F);
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cellHeaderID,
            this.cellHeaderName,
            this.cellHeaderStatus});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // cellHeaderID
            // 
            this.cellHeaderID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.cellHeaderID.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.cellHeaderID.ForeColor = System.Drawing.Color.White;
            this.cellHeaderID.Multiline = true;
            this.cellHeaderID.Name = "cellHeaderID";
            this.cellHeaderID.StylePriority.UseBackColor = false;
            this.cellHeaderID.StylePriority.UseFont = false;
            this.cellHeaderID.StylePriority.UseForeColor = false;
            this.cellHeaderID.Text = "Student ID";
            this.cellHeaderID.Weight = 1.640416394356917D;
            // 
            // cellHeaderName
            // 
            this.cellHeaderName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.cellHeaderName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.cellHeaderName.ForeColor = System.Drawing.Color.White;
            this.cellHeaderName.Multiline = true;
            this.cellHeaderName.Name = "cellHeaderName";
            this.cellHeaderName.StylePriority.UseBackColor = false;
            this.cellHeaderName.StylePriority.UseFont = false;
            this.cellHeaderName.StylePriority.UseForeColor = false;
            this.cellHeaderName.Text = "Full Name";
            this.cellHeaderName.Weight = 3.8529324204432975D;
            // 
            // cellHeaderStatus
            // 
            this.cellHeaderStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.cellHeaderStatus.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F);
            this.cellHeaderStatus.ForeColor = System.Drawing.Color.White;
            this.cellHeaderStatus.Multiline = true;
            this.cellHeaderStatus.Name = "cellHeaderStatus";
            this.cellHeaderStatus.StylePriority.UseBackColor = false;
            this.cellHeaderStatus.StylePriority.UseFont = false;
            this.cellHeaderStatus.StylePriority.UseForeColor = false;
            this.cellHeaderStatus.Text = "Status";
            this.cellHeaderStatus.Weight = 1.9499848626391543D;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(37.66666F, 138.7917F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(125F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Academic Year: ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(311.8747F, 138.7917F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(114.5833F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Semester:";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(605.6247F, 138.7917F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(123.9583F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Status:";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // OfficeReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader,
            this.PageHeader});
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(27F, 31F, 0F, 389.5833F);
            this.Version = "23.2";
            xrWatermark1.Id = "Watermark1";
            this.Watermarks.Add(xrWatermark1);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell cellHeaderID;
        private DevExpress.XtraReports.UI.XRTableCell cellHeaderName;
        private DevExpress.XtraReports.UI.XRTableCell cellHeaderStatus;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        public DevExpress.XtraReports.UI.XRTableCell cellReportName;
        public DevExpress.XtraReports.UI.XRTableCell cellReportStatus;
        public DevExpress.XtraReports.UI.XRTableCell cellReportID;
        public DevExpress.XtraReports.UI.XRLabel lblReportStatus;
        public DevExpress.XtraReports.UI.XRLabel lblReportSem;
        public DevExpress.XtraReports.UI.XRLabel lblReportYear;
        public DevExpress.XtraReports.UI.XRLabel xrLabel4;
        public DevExpress.XtraReports.UI.XRLabel xrLabel5;
        public DevExpress.XtraReports.UI.XRLabel xrLabel6;
    }
}
