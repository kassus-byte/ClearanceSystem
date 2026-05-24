namespace SchoolClearanceSystem
{
    partial class UserInfoForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUploadPhoto = new DevExpress.XtraEditors.SimpleButton();
            this.pePhoto = new DevExpress.XtraEditors.PictureEdit();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.txtFullName = new DevExpress.XtraEditors.TextEdit();
            this.cbProgram = new System.Windows.Forms.ComboBox();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl15 = new DevExpress.XtraEditors.PanelControl();
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDateCreated = new DevExpress.XtraEditors.TextEdit();
            this.lbl = new DevExpress.XtraEditors.LabelControl();
            this.cbRole = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pePhoto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).BeginInit();
            this.panelControl15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUploadPhoto
            // 
            this.btnUploadPhoto.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnUploadPhoto.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadPhoto.Appearance.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnUploadPhoto.Appearance.Options.UseBackColor = true;
            this.btnUploadPhoto.Appearance.Options.UseFont = true;
            this.btnUploadPhoto.Appearance.Options.UseForeColor = true;
            this.btnUploadPhoto.Location = new System.Drawing.Point(712, 487);
            this.btnUploadPhoto.Margin = new System.Windows.Forms.Padding(4);
            this.btnUploadPhoto.Name = "btnUploadPhoto";
            this.btnUploadPhoto.Size = new System.Drawing.Size(97, 32);
            this.btnUploadPhoto.TabIndex = 29;
            this.btnUploadPhoto.Text = "Upload";
            this.btnUploadPhoto.Click += new System.EventHandler(this.btnUploadPhoto_Click);
            // 
            // pePhoto
            // 
            this.pePhoto.Location = new System.Drawing.Point(662, 527);
            this.pePhoto.Margin = new System.Windows.Forms.Padding(4);
            this.pePhoto.Name = "pePhoto";
            this.pePhoto.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pePhoto.Size = new System.Drawing.Size(492, 181);
            this.pePhoto.TabIndex = 28;
            this.pePhoto.EditValueChanged += new System.EventHandler(this.pePhoto_EditValueChanged);
            // 
            // cbYear
            // 
            this.cbYear.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbYear.FormattingEnabled = true;
            this.cbYear.Items.AddRange(new object[] {
            "I",
            "II",
            "III",
            "IV",
            "Irregular"});
            this.cbYear.Location = new System.Drawing.Point(30, 422);
            this.cbYear.Margin = new System.Windows.Forms.Padding(4);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(541, 28);
            this.cbYear.TabIndex = 26;
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(662, 220);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtFullName.Properties.Appearance.Options.UseFont = true;
            this.txtFullName.Size = new System.Drawing.Size(523, 38);
            this.txtFullName.TabIndex = 25;
            // 
            // cbProgram
            // 
            this.cbProgram.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbProgram.FormattingEnabled = true;
            this.cbProgram.Items.AddRange(new object[] {
            "BSIT"});
            this.cbProgram.Location = new System.Drawing.Point(662, 340);
            this.cbProgram.Margin = new System.Windows.Forms.Padding(4);
            this.cbProgram.Name = "cbProgram";
            this.cbProgram.Size = new System.Drawing.Size(522, 28);
            this.cbProgram.TabIndex = 24;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(30, 220);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtUserID.Properties.Appearance.Options.UseFont = true;
            this.txtUserID.Size = new System.Drawing.Size(541, 38);
            this.txtUserID.TabIndex = 23;
            this.txtUserID.EditValueChanged += new System.EventHandler(this.txtUserID_EditValueChanged);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(1070, 116);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(164, 53);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Appearance.Options.UseForeColor = true;
            this.btnCancel.Location = new System.Drawing.Point(884, 116);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(164, 53);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Appearance.Options.UseForeColor = true;
            this.labelControl8.Location = new System.Drawing.Point(660, 491);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(44, 21);
            this.labelControl8.TabIndex = 20;
            this.labelControl8.Text = "Photo";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Appearance.Options.UseForeColor = true;
            this.labelControl7.Location = new System.Drawing.Point(30, 394);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(32, 21);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Year";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(662, 311);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(63, 21);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "Program";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(662, 192);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(43, 21);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Name";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(30, 192);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(53, 21);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "User ID";
            // 
            // panelControl15
            // 
            this.panelControl15.Appearance.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panelControl15.Appearance.Options.UseBackColor = true;
            this.panelControl15.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl15.Controls.Add(this.lblTitle);
            this.panelControl15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl15.Location = new System.Drawing.Point(0, 0);
            this.panelControl15.Margin = new System.Windows.Forms.Padding(4);
            this.panelControl15.Name = "panelControl15";
            this.panelControl15.Size = new System.Drawing.Size(1300, 96);
            this.panelControl15.TabIndex = 30;
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Appearance.Options.UseForeColor = true;
            this.lblTitle.Location = new System.Drawing.Point(42, 23);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(265, 46);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Edit Information";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(662, 389);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 21);
            this.labelControl1.TabIndex = 32;
            this.labelControl1.Text = "Date Created";
            // 
            // txtDateCreated
            // 
            this.txtDateCreated.Location = new System.Drawing.Point(662, 417);
            this.txtDateCreated.Margin = new System.Windows.Forms.Padding(4);
            this.txtDateCreated.Name = "txtDateCreated";
            this.txtDateCreated.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.txtDateCreated.Properties.Appearance.Options.UseFont = true;
            this.txtDateCreated.Size = new System.Drawing.Size(523, 38);
            this.txtDateCreated.TabIndex = 33;
            // 
            // lbl
            // 
            this.lbl.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(80)))), ((int)(((byte)(65)))));
            this.lbl.Appearance.Options.UseFont = true;
            this.lbl.Appearance.Options.UseForeColor = true;
            this.lbl.Location = new System.Drawing.Point(30, 306);
            this.lbl.Margin = new System.Windows.Forms.Padding(4);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(33, 21);
            this.lbl.TabIndex = 34;
            this.lbl.Text = "Role";
            // 
            // cbRole
            // 
            this.cbRole.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.cbRole.FormattingEnabled = true;
            this.cbRole.Items.AddRange(new object[] {
            "Student",
            "Treasurer",
            "Technical Office",
            "SSG",
            "Admin"});
            this.cbRole.Location = new System.Drawing.Point(30, 340);
            this.cbRole.Margin = new System.Windows.Forms.Padding(4);
            this.cbRole.Name = "cbRole";
            this.cbRole.Size = new System.Drawing.Size(541, 28);
            this.cbRole.TabIndex = 35;
            // 
            // UserInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 742);
            this.Controls.Add(this.cbRole);
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txtDateCreated);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl15);
            this.Controls.Add(this.btnUploadPhoto);
            this.Controls.Add(this.pePhoto);
            this.Controls.Add(this.cbYear);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.cbProgram);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UserInfoForm";
            this.Text = "UserInfoForm";
            this.Load += new System.EventHandler(this.UserInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pePhoto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFullName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl15)).EndInit();
            this.panelControl15.ResumeLayout(false);
            this.panelControl15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDateCreated.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnUploadPhoto;
        private DevExpress.XtraEditors.PictureEdit pePhoto;
        private System.Windows.Forms.ComboBox cbYear;
        private DevExpress.XtraEditors.TextEdit txtFullName;
        private System.Windows.Forms.ComboBox cbProgram;
        private DevExpress.XtraEditors.TextEdit txtUserID;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl15;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDateCreated;
        private DevExpress.XtraEditors.LabelControl lbl;
        private System.Windows.Forms.ComboBox cbRole;
    }
}