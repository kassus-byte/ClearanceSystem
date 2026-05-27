namespace SchoolClearanceSystem
{
    partial class StudentClearanceSlip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StudentClearanceSlip));
            DevExpress.XtraReports.UI.XRWatermark xrWatermark1 = new DevExpress.XtraReports.UI.XRWatermark();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.cellStatusTreasurer = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.cellStatusSSG = new DevExpress.XtraReports.UI.XRLabel();
            this.cellStatusTechnical = new DevExpress.XtraReports.UI.XRLabel();
            this.cellSem = new DevExpress.XtraReports.UI.XRLabel();
            this.cellYearLevel = new DevExpress.XtraReports.UI.XRLabel();
            this.cellStudentID = new DevExpress.XtraReports.UI.XRLabel();
            this.cellFullName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.cellSchoolYear = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCellTechnical = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 381.25F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel6,
            this.cellStatusTreasurer,
            this.xrLabel4,
            this.cellStatusSSG,
            this.cellStatusTechnical,
            this.lblCellTechnical,
            this.cellYearLevel,
            this.cellStudentID,
            this.cellFullName});
            this.Detail.HeightF = 256.25F;
            this.Detail.Name = "Detail";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(589.1665F, 162.4583F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(218.7502F, 27.16667F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Treasurer";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // cellStatusTreasurer
            // 
            this.cellStatusTreasurer.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellStatusTreasurer.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellStatusTreasurer.LocationFloat = new DevExpress.Utils.PointFloat(641.2499F, 139.4583F);
            this.cellStatusTreasurer.Multiline = true;
            this.cellStatusTreasurer.Name = "cellStatusTreasurer";
            this.cellStatusTreasurer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellStatusTreasurer.SizeF = new System.Drawing.SizeF(116.6666F, 23F);
            this.cellStatusTreasurer.StylePriority.UseBorders = false;
            this.cellStatusTreasurer.StylePriority.UseFont = false;
            this.cellStatusTreasurer.Text = "NOT CLEARED";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(323.5415F, 162.4583F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(218.7502F, 27.16667F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "SSG";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // cellStatusSSG
            // 
            this.cellStatusSSG.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellStatusSSG.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellStatusSSG.LocationFloat = new DevExpress.Utils.PointFloat(375.6248F, 139.4583F);
            this.cellStatusSSG.Multiline = true;
            this.cellStatusSSG.Name = "cellStatusSSG";
            this.cellStatusSSG.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellStatusSSG.SizeF = new System.Drawing.SizeF(116.6666F, 23F);
            this.cellStatusSSG.StylePriority.UseBorders = false;
            this.cellStatusSSG.StylePriority.UseFont = false;
            this.cellStatusSSG.Text = "NOT CLEARED";
            // 
            // cellStatusTechnical
            // 
            this.cellStatusTechnical.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellStatusTechnical.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellStatusTechnical.LocationFloat = new DevExpress.Utils.PointFloat(102.0832F, 139.4583F);
            this.cellStatusTechnical.Multiline = true;
            this.cellStatusTechnical.Name = "cellStatusTechnical";
            this.cellStatusTechnical.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellStatusTechnical.SizeF = new System.Drawing.SizeF(116.6666F, 23F);
            this.cellStatusTechnical.StylePriority.UseBorders = false;
            this.cellStatusTechnical.StylePriority.UseFont = false;
            this.cellStatusTechnical.Text = "NOT CLEARED";
            // 
            // cellSem
            // 
            this.cellSem.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellSem.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellSem.LocationFloat = new DevExpress.Utils.PointFloat(624.5831F, 84.04166F);
            this.cellSem.Multiline = true;
            this.cellSem.Name = "cellSem";
            this.cellSem.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellSem.SizeF = new System.Drawing.SizeF(160.4167F, 23.00002F);
            this.cellSem.StylePriority.UseBorders = false;
            this.cellSem.StylePriority.UseFont = false;
            this.cellSem.Text = "_________________________";
            // 
            // cellYearLevel
            // 
            this.cellYearLevel.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellYearLevel.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellYearLevel.LocationFloat = new DevExpress.Utils.PointFloat(675.6249F, 32.49998F);
            this.cellYearLevel.Multiline = true;
            this.cellYearLevel.Name = "cellYearLevel";
            this.cellYearLevel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellYearLevel.SizeF = new System.Drawing.SizeF(92.70847F, 23F);
            this.cellYearLevel.StylePriority.UseBorders = false;
            this.cellYearLevel.StylePriority.UseFont = false;
            this.cellYearLevel.Text = "________________";
            // 
            // cellStudentID
            // 
            this.cellStudentID.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellStudentID.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellStudentID.LocationFloat = new DevExpress.Utils.PointFloat(436.0415F, 32.49998F);
            this.cellStudentID.Multiline = true;
            this.cellStudentID.Name = "cellStudentID";
            this.cellStudentID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellStudentID.SizeF = new System.Drawing.SizeF(91.6669F, 23F);
            this.cellStudentID.StylePriority.UseBorders = false;
            this.cellStudentID.StylePriority.UseFont = false;
            this.cellStudentID.Text = "________________";
            // 
            // cellFullName
            // 
            this.cellFullName.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellFullName.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellFullName.LocationFloat = new DevExpress.Utils.PointFloat(148.9582F, 32.49998F);
            this.cellFullName.Multiline = true;
            this.cellFullName.Name = "cellFullName";
            this.cellFullName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellFullName.SizeF = new System.Drawing.SizeF(194.7917F, 23F);
            this.cellFullName.StylePriority.UseBorders = false;
            this.cellFullName.StylePriority.UseFont = false;
            this.cellFullName.Text = "_________________________________________";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(100F, 100F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.cellSem,
            this.xrLabel10,
            this.cellSchoolYear,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPictureBox1});
            this.ReportHeader.HeightF = 172.9167F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // cellSchoolYear
            // 
            this.cellSchoolYear.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.cellSchoolYear.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.cellSchoolYear.LocationFloat = new DevExpress.Utils.PointFloat(698.5416F, 61.04164F);
            this.cellSchoolYear.Multiline = true;
            this.cellSchoolYear.Name = "cellSchoolYear";
            this.cellSchoolYear.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.cellSchoolYear.SizeF = new System.Drawing.SizeF(106.2501F, 23.00001F);
            this.cellSchoolYear.StylePriority.UseBorders = false;
            this.cellSchoolYear.StylePriority.UseFont = false;
            this.cellSchoolYear.Text = "________________";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(122.9167F, 76.99998F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(318.75F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "COLLEGE OF COMPUTER STUDIES";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Segoe UI", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(122.9167F, 46.54166F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(318.75F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "DMC COLLEGE FOUNDATION INC.";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Segoe UI", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(122.9167F, 10.00001F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(184.375F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "CLEARANCE SLIP";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(68.74987F, 32.49998F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(80.20834F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Full Name:";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(397.4998F, 32.49998F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(38.54166F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "ID #:";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(586.0415F, 32.49998F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(89.58331F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Year Level:";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(586.0415F, 84.04166F);
            this.xrLabel9.Multiline = true;
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(38.54166F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Sem:";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new DevExpress.Drawing.DXFont("Segoe UI", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(586.0415F, 61.04164F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(112.5001F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Academic Year:";
            // 
            // lblCellTechnical
            // 
            this.lblCellTechnical.Font = new DevExpress.Drawing.DXFont("Segoe UI", 12F);
            this.lblCellTechnical.LocationFloat = new DevExpress.Utils.PointFloat(49.99987F, 162.4583F);
            this.lblCellTechnical.Multiline = true;
            this.lblCellTechnical.Name = "lblCellTechnical";
            this.lblCellTechnical.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCellTechnical.SizeF = new System.Drawing.SizeF(218.7502F, 27.16667F);
            this.lblCellTechnical.StylePriority.UseFont = false;
            this.lblCellTechnical.StylePriority.UseTextAlignment = false;
            this.lblCellTechnical.Text = "Technical Office";
            this.lblCellTechnical.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // StudentClearanceSlip
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail,
            this.ReportHeader});
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(25F, 0F, 100F, 381.25F);
            this.Version = "23.2";
            xrWatermark1.Id = "Watermark1";
            this.Watermarks.Add(xrWatermark1);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel cellSem;
        private DevExpress.XtraReports.UI.XRLabel cellYearLevel;
        private DevExpress.XtraReports.UI.XRLabel cellStudentID;
        private DevExpress.XtraReports.UI.XRLabel cellFullName;
        private DevExpress.XtraReports.UI.XRLabel cellSchoolYear;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel cellStatusTechnical;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel cellStatusTreasurer;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel cellStatusSSG;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel lblCellTechnical;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
    }
}


